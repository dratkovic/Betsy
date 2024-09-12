<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col md="4">
        <v-card class="pa-4">
          <v-card-title class="text-center">Login</v-card-title>
          <v-card-item>
            <v-form ref="refForm" novalidate @submit.prevent="submit">
              <v-text-field
                v-model="loginRequest.email"
                label="Email"
                prepend-inner-icon="mdi-email"
                :rules="[requiredValidator, emailValidator]"
              ></v-text-field>

              <v-text-field
                v-model="loginRequest.password"
                label="Password"
                prepend-inner-icon="mdi-key"
                :rules="[passwordValidator]"
                type="password"
              ></v-text-field>

              <v-btn
                block
                class="mt-2"
                color="red-darken-1"
                type="submit"
                variant="elevated"
              >
                Submit
              </v-btn>
            </v-form>
          </v-card-item>

          <v-card-actions>
              <v-btn block to="/register"> Register </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useAppStore } from "@/stores/app.store";
import { ValidationError } from "@/types/models/types.betsy";
import { emailValidator, passwordValidator, requiredValidator } from "@/utils/validators";
import { ref } from "vue";

const appStore = useAppStore();
const refForm = ref()

const loginRequest = ref({
  email: "jerry@betsy.hr",
  password: "P@ssword12",
});

const isLoading = ref(false);

async function submit() {
   const {valid} = await refForm.value.validate()

   if(!valid) 
     return;
   
  isLoading.value = true;
  
    appStore.login(loginRequest.value).catch(error => {
      if(error as ValidationError)
        appStore.showError(error.description);
      else
      appStore.showError("Cannot comunicate with the server. Please try again later.");
    }).finally(() => {
      isLoading.value = false;
    });
}

</script>
<route lang="yaml">
meta:
  layout: guest
</route>
