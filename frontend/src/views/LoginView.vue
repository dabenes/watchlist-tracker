<template>
    <div class="bg-danger gradient-custom container-fluid vh-100 py-5">
      <div class="d-flex flex-column text-center bg-dark justify-content-center mx-auto col-4 text-white rounded my-5 py-3 opacity-75">
        <form @submit.prevent="login">
          <!-- Nombre de usuario -->
          <div class="form-outline mb-4 px-5">
            <label class="form-label" for="username">Nombre de usuario</label>
            <input type="text" id="username" class="form-control" v-model="username" required />
          </div>
  
          <!-- Contraseña -->
          <div class="form-outline mb-4 px-5">
            <label class="form-label" for="password">Contraseña</label>
            <input type="password" id="password" class="form-control" v-model="password" required />
          </div>
  
          <!-- Recordar sesión -->
          <div class="row mb-4">
            <div class="col d-flex justify-content-center">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" id="rememberMe" v-model="rememberMe" />
                <label class="form-check-label" for="rememberMe"> Recordarme </label>
              </div>
            </div>
  
            <div class="col">
              <!-- Forgot Password Link (sin funcionalidad aún) -->
              <a href="#" class="link-danger" @click.prevent="forgotPassword">¿Olvidaste tu contraseña?</a>
            </div>
          </div>
  
          <!-- Botón de inicio de sesión -->
          <button type="submit" class="btn btn-danger btn-block mb-4">Iniciar sesión</button>
  
          <!-- Register link -->
          <div class="text-center">
            <p>¿No tienes cuenta? <a href="#" class="link-danger" @click.prevent="goToRegister">Regístrate</a></p>
          </div>
        </form>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        username: '',
        password: '',
        rememberMe: false
      };
    },
    methods: {
      async login() {
        try {
          const response = await axios.post('/auth/login', {
            username: this.username,
            password: this.password
          });
  
          // Guardar token si la autenticación es exitosa
          localStorage.setItem('token', response.data.token);
          this.$router.push('/listas'); // Redirigir a la lista de seguimiento
        } catch (error) {
          console.error('Error en el login', error);
          alert('Usuario o contraseña incorrectos.');
        }
      },
      forgotPassword() {
        alert('Funcionalidad de recuperación de contraseña aún no implementada.');
      },
      goToRegister() {
        alert('Funcionalidad de registro aún no implementada.');
      }
    }
  };
  </script>
  