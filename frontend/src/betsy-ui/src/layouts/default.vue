<template>
  <v-app>
    <v-layout class="rounded rounded-md">
    <v-app-bar density="compact">
      <v-btn to="/">Dashboard</v-btn>
      <v-btn to="/tickets">Tickets</v-btn>
      <v-spacer></v-spacer>

      <p>Hello {{appStore.user?.firstName }} {{ appStore.user?.lastName }}</p>
      <v-btn icon @click="changeTheme">
        <v-icon :icon="darkTheme ? 'mdi-weather-night' : 'mdi-weather-sunny'"></v-icon>
      </v-btn>
      
      <v-btn size="small" text="Logout" to="/login" variant="outlined" @click="logout()"> </v-btn>
    </v-app-bar>

    
    <RouterView >
    </RouterView>
    
    <app-footer/> 
    </v-layout>
  </v-app>
  
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { useTheme } from "vuetify";
import { useRouter } from "vue-router";
import { useAppStore } from "@/stores/app.store";

const router = useRouter();
const darkTheme = ref(true)
const theme = useTheme();
const appStore = useAppStore();

function changeTheme() {
  darkTheme.value = !darkTheme.value;
  theme.global.name.value = darkTheme.value ? "dark" : "light";
}

function logout() {
  appStore.logout();
  router.push("/login");
}

</script>
