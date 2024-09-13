<template>
  <v-app>
    <v-overlay class="align-center justify-center" :model-value="appStore.isLoadingFromServer">
      <v-progress-circular
        v-if="appStore.isLoadingFromServer"
        color="white"
        indeterminate
      ></v-progress-circular>
    </v-overlay>
    
      <router-view />
    
        <v-snackbar  v-model="appStore.errorSnackbar" color="red-darken-1" timeout="2500"
      >{{ appStore.errorMessage }}</v-snackbar
    >
  </v-app>
</template>

<script lang="ts" setup>
  import { useAppStore } from './stores/app.store';

  const appStore = useAppStore();
</script>
