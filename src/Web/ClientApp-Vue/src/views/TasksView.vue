<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import {
  CreateTodoItemCommand,
  CreateTodoListCommand,
  type ITodoItemDto,
  type ITodoListDto,
  TodoItemDto,
  TodoItemsClient,
  TodoListDto,
  TodoListsClient,
  UpdateTodoItemCommand,
} from '../web-api-client';

const listsClient = new TodoListsClient();
const itemsClient = new TodoItemsClient();

type ListsResult = Awaited<ReturnType<TodoListsClient['getTodoLists']>>;
type TodoList = ITodoListDto;
type TodoItem = ITodoItemDto;

const lists = ref<TodoList[] | null>(null);
const priorityLevels = ref<NonNullable<ListsResult['priorityLevels']>>([]);
const colours = ref<NonNullable<ListsResult['colours']>>([]);
const selectedListId = ref<number | null>(null);
const newListTitle = ref('');
const newItemTitle = ref('');
const loading = ref(true);
const error = ref('');

const selectedList = computed(() =>
  lists.value?.find((list) => list.id === selectedListId.value) ?? null,
);

onMounted(async () => {
  try {
    const result = await listsClient.getTodoLists();
    const resultLists = result.lists ?? [];
    lists.value = resultLists;
    priorityLevels.value = result.priorityLevels ?? [];
    colours.value = result.colours ?? [];
    selectedListId.value = resultLists[0]?.id ?? null;
  } catch {
    error.value = 'Unable to load your task lists. Please try again later.';
  } finally {
    loading.value = false;
  }
});

async function createList(): Promise<void> {
  const title = newListTitle.value.trim();
  if (!title || !lists.value) return;

  try {
    const colour = colours.value[0]?.code ?? '';
    const id = await listsClient.createTodoList(new CreateTodoListCommand({ title, colour }));
    const newList = new TodoListDto({ id, title, colour, items: [] });
    lists.value = [...lists.value, newList];
    selectedListId.value = id;
    newListTitle.value = '';
    error.value = '';
  } catch {
    error.value = 'Unable to create the task list.';
  }
}

async function deleteList(): Promise<void> {
  const list = selectedList.value;
  if (!list || !lists.value) return;

  try {
    if (list.id === undefined) return;
    await listsClient.deleteTodoList(list.id);
    lists.value = lists.value.filter((entry) => entry.id !== list.id);
    selectedListId.value = lists.value[0]?.id ?? null;
    error.value = '';
  } catch {
    error.value = 'Unable to delete the task list.';
  }
}

async function createItem(): Promise<void> {
  const title = newItemTitle.value.trim();
  const list = selectedList.value;
  if (!title || !list || list.id === undefined || !lists.value) return;

  try {
    const id = await itemsClient.createTodoItem(new CreateTodoItemCommand({ title, listId: list.id }));
    const item = new TodoItemDto({
      id,
      listId: list.id,
      title,
      done: false,
      priority: priorityLevels.value[0]?.id ?? 0,
    });
    lists.value = lists.value.map((entry) =>
      entry.id === list.id ? { ...entry, items: [...(entry.items ?? []), item] } : entry,
    );
    newItemTitle.value = '';
    error.value = '';
  } catch {
    error.value = 'Unable to create the task.';
  }
}

async function toggleItem(item: TodoItem): Promise<void> {
  if (!lists.value || item.id === undefined) return;
  const updatedItem = new TodoItemDto({ ...item, done: !item.done });
  lists.value = lists.value.map((list) =>
    list.id === item.listId
      ? { ...list, items: (list.items ?? []).map((entry) => entry.id === item.id ? updatedItem : entry) }
      : list,
  );

  try {
    await itemsClient.updateTodoItem(item.id, new UpdateTodoItemCommand({
      id: item.id,
      title: updatedItem.title,
      done: updatedItem.done,
    }));
    error.value = '';
  } catch {
    error.value = 'Unable to update the task.';
  }
}

async function deleteItem(item: TodoItem): Promise<void> {
  if (!lists.value || item.id === undefined) return;
  try {
    await itemsClient.deleteTodoItem(item.id);
    lists.value = lists.value.map((list) =>
      list.id === item.listId
        ? { ...list, items: (list.items ?? []).filter((entry) => entry.id !== item.id) }
        : list,
    );
    error.value = '';
  } catch {
    error.value = 'Unable to delete the task.';
  }
}

function remainingItems(list: TodoList): number {
  return (list.items ?? []).filter((item) => !item.done).length;
}
</script>

<template>
  <section>
    <hgroup>
      <h1>Tasks</h1>
      <p>Manage your todo lists and tasks.</p>
    </hgroup>

    <span v-if="loading" aria-busy="true">Loading…</span>
    <p v-else-if="error && !lists" class="error" role="alert">{{ error }}</p>
    <div v-else-if="lists" class="todo-layout">
      <aside class="todo-sidebar" aria-label="Task lists">
        <div class="todo-panel-header">
          <h2>Lists</h2>
        </div>
        <ul>
          <li v-for="list in lists" :key="list.id" :aria-current="list.id === selectedListId ? 'true' : undefined">
            <button
              class="list-option"
              type="button"
              :aria-pressed="list.id === selectedListId"
              @click="selectedListId = list.id ?? null"
            >
              <span>{{ list.title }}</span>
              <small>{{ remainingItems(list) }}</small>
            </button>
          </li>
        </ul>
        <form class="new-list-form" @submit.prevent="createList">
          <label class="visually-hidden" for="new-list-title">New list name</label>
          <input id="new-list-title" v-model="newListTitle" placeholder="New list name" required />
          <button type="submit" class="secondary">Add list</button>
        </form>
      </aside>

      <div class="todo-main">
        <template v-if="selectedList">
          <div class="todo-panel-header">
            <h2>{{ selectedList.title }}</h2>
            <button type="button" class="danger" @click="deleteList">Delete list</button>
          </div>
          <p v-if="error" class="error" role="alert">{{ error }}</p>
          <ul class="todo-items">
            <li v-for="item in selectedList.items ?? []" :key="item.id" class="todo-item">
              <input
                :id="`todo-${item.id}`"
                type="checkbox"
                :checked="item.done"
                @change="toggleItem(item)"
              />
              <label :for="`todo-${item.id}`" :class="['todo-item-text', { 'todo-done': item.done }]">
                {{ item.title }}
              </label>
              <button type="button" class="icon-btn" :aria-label="`Delete ${item.title}`" @click="deleteItem(item)">×</button>
            </li>
            <li v-if="(selectedList.items ?? []).length === 0" class="empty-list">No tasks yet.</li>
          </ul>
          <form class="new-item-form" @submit.prevent="createItem">
            <label class="visually-hidden" for="new-task-title">New task</label>
            <input id="new-task-title" v-model="newItemTitle" placeholder="Add a task" required />
            <button type="submit">Add task</button>
          </form>
        </template>
        <p v-else>No task lists yet. Create one to get started.</p>
      </div>
    </div>
  </section>
</template>
