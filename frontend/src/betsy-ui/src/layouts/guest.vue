<template>
  <v-app>
    <v-main>
       <RouterView v-slot="{ Component }">
        <v-fade-transition>
          <component :is="Component" />
        </v-fade-transition>
      </RouterView>
    </v-main>
  </v-app>   
  <app-footer/> 
</template>

<script lang="ts" setup>
//
</script>
