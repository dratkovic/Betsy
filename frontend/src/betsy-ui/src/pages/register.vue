<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col md="4">
        <v-card class="pa-4">
          <v-card-title class="text-center">Register</v-card-title>
          <v-card-item>
            <v-form ref="refForm" novalidate @submit.prevent="submit">
              <v-text-field
                v-model="registerRequest.firstName"
                label="First Name"
                prepend-inner-icon="mdi-account"
                :rules="[requiredValidator]"
              ></v-text-field>

              <v-text-field
                v-model="registerRequest.lastName"
                label="Last Name"
                prepend-inner-icon="mdi-account"
                :rules="[requiredValidator]"
              ></v-text-field>

              <v-text-field
                v-model="registerRequest.email"
                label="Email"
                prepend-inner-icon="mdi-email"
                :rules="[requiredValidator, emailValidator]"
              ></v-text-field>

              <v-text-field
                v-model="registerRequest.password"
                :append-inner-icon="isPasswordVisible ? 'mdi-eye-off-outline' : 'mdi-eye-outline'"
                label="Password"
                prepend-inner-icon="mdi-key"
                :rules="[passwordValidator]"
                :type="isPasswordVisible ? 'text' : 'password'"
                @click:append-inner="isPasswordVisible = !isPasswordVisible"
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
              <v-btn block to="/login"> Login instead </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useAppStore } from "@/stores/app.store";
import { ValidationError } from "@/types/types.betsy";
import { emailValidator, passwordValidator, requiredValidator } from "@/utils/validators";
import { ref } from "vue";

const appStore = useAppStore();
const refForm = ref()

const registerRequest = ref({
  firstName: "",
  lastName: "",
  email: "",
  password: "",
});

const isPasswordVisible = ref(false);

const router = useRouter()

async function submit() {
   const {valid} = await refForm.value.validate()

   if(!valid) 
     return;
     
  appStore.register(registerRequest.value)
  .then(() => {
    router.push("/");
  })
  .catch(error => {
    if(error as ValidationError)
      appStore.showError(error.description);
    else
      appStore.showError("Cannot comunicate with the server. Please try again later.");
    })
}

</script>
<route lang="yaml">
meta:
  layout: guest
</route>
