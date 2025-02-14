<template>
  <div class="container-fluid bg-dark text-white vh-100 p-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>Mis Listas</h2>
      <button class="btn btn-danger" @click="crearLista">+ Crear Lista</button>
      <button class="btn btn-secondary" @click="cerrarSesion">Cerrar Sesión</button>
    </div>

    <!-- Selector de lista -->
    <div class="mb-3">
      <label for="listaSeleccionada">Selecciona una lista:</label>
      <select v-model="listaSeleccionada" class="form-select bg-dark text-white">
        <option v-for="lista in listas" :key="lista.id" :value="lista.id">
          {{ lista.nombre }}
        </option>
      </select>
    </div>

    <!-- Películas en la lista seleccionada -->
    <div v-if="listaSeleccionada">
      <h3>Películas en la lista</h3>
      <ul class="list-group">
        <li v-for="pelicula in peliculasEnLista" :key="pelicula.id" class="list-group-item bg-dark text-white d-flex justify-content-between">
          <span @click="mostrarDetalles(pelicula)">{{ pelicula.titulo }} ({{ pelicula.anio }})</span>
          <button class="btn btn-danger btn-sm" @click="eliminarDeLista(pelicula.id)">Eliminar</button>
        </li>
      </ul>
    </div>
    
    <!-- Buscador -->
    <div class="mt-4">
      <input v-model="busqueda" type="text" class="form-control bg-dark text-white" placeholder="Buscar película o serie..." />
    </div>

    <!-- Listado de películas -->
    <div class="mt-3">
      <h3>Películas disponibles</h3>
      <ul class="list-group">
        <li v-for="pelicula in peliculasFiltradas" :key="pelicula.id" class="list-group-item bg-dark text-white d-flex justify-content-between">
          <span @click="mostrarDetalles(pelicula)">{{ pelicula.titulo }} ({{ pelicula.anio }})</span>
          <button class="btn btn-success btn-sm" @click="agregarALista(pelicula.id)">Añadir</button>
        </li>
      </ul>
    </div>

    <!-- CardView para detalles -->
    <CardView v-if="peliculaSeleccionada" :movie="peliculaSeleccionada" @close="peliculaSeleccionada = null" />
  </div>
</template>

<script>
import axios from "axios";
import CardView from "@/components/CardView.vue";

export default {
  components: { CardView },
  data() {
    return {
      listas: [],
      peliculas: [],
      listaSeleccionada: null,
      peliculaSeleccionada: null,
      busqueda: "",
    };
  },
  computed: {
    peliculasFiltradas() {
      return this.peliculas.filter(p =>
        p.titulo.toLowerCase().includes(this.busqueda.toLowerCase())
      );
    },
    peliculasEnLista() {
      if (!this.listaSeleccionada) return [];
      return this.peliculas.filter(p => p.listaId === this.listaSeleccionada);
    }
  },
  methods: {
    async cargarDatos() {
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          console.error("No hay token, redirigiendo al login...");
          this.$router.push("/");
          return;
        }

        const listasRes = await axios.get("http://localhost:5176/api/listas", {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.listas = listasRes.data;

        const peliculasRes = await axios.get("http://localhost:5176/api/peliculas", {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.peliculas = peliculasRes.data;
      } catch (error) {
        console.error("Error al cargar datos:", error);
      }
    },
    async crearLista() {
      const nombre = prompt("Nombre de la nueva lista:");
      if (!nombre) return;
      try {
        const token = localStorage.getItem("token");
        await axios.post("http://localhost:5176/api/listas", { nombre }, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.cargarDatos();
      } catch (error) {
        console.error("Error al crear lista:", error);
      }
    },
    async agregarALista(peliculaId) {
      if (!this.listaSeleccionada) return;
      try {
        const token = localStorage.getItem("token");
        await axios.post("http://localhost:5176/api/listas/agregar", { listaId: this.listaSeleccionada, peliculaId }, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.cargarDatos();
      } catch (error) {
        console.error("Error al añadir película:", error);
      }
    },
    async eliminarDeLista(peliculaId) {
      if (!this.listaSeleccionada) return;
      try {
        const token = localStorage.getItem("token");
        await axios.post("http://localhost:5176/api/listas/eliminar", { listaId: this.listaSeleccionada, peliculaId }, {
          headers: { Authorization: `Bearer ${token}` }
        });
        this.cargarDatos();
      } catch (error) {
        console.error("Error al eliminar película:", error);
      }
    },
    mostrarDetalles(pelicula) {
      this.peliculaSeleccionada = pelicula;
    },
    cerrarSesion() {
      localStorage.removeItem("token");
      this.$router.push("/");
    }
  },
  mounted() {
    this.cargarDatos();
  }
};
</script>

<style scoped>
.container-fluid {
  padding: 20px;
}
.list-group-item {
  cursor: pointer;
}
</style>
