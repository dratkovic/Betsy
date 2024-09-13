<template>
  <v-app>
    <v-main>
       
      <router-view v-slot="{ Component }">
        <v-scale-transition>
          <component :is="Component" />
        </v-scale-transition>
      </router-view>
    
    </v-main>
    <app-footer/> 
  </v-app>     
</template>

<script lang="ts" setup>
//
</script>
