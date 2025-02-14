<template>
  <div class="card bg-dark text-white border-0 shadow-lg rounded-3">
    <img :src="movie.posterUrl" class="card-img-top" alt="Poster de la película o serie" />
    <div class="card-body text-center">
      <h5 class="card-title fw-bold">{{ movie.titulo }}</h5>
      <p class="card-text">
        <strong>{{ movie.tipo === 'serie' ? 'Serie' : 'Película' }}</strong>
        <br />
        <small>{{ movie.anio }}</small>
      </p>
      <div v-if="movie.tipo === 'serie'">
        <p class="card-text"><small>Temporadas: {{ movie.temporadas }}</small></p>
      </div>
      <button class="btn btn-danger w-100 mt-2 fw-bold" @click="toggleLista">
        {{ estaEnLista ? "❌ Eliminar" : "➕ Añadir" }}
      </button>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    movie: Object,
    estaEnLista: Boolean,
  },
  methods: {
    toggleLista() {
      this.$emit(this.estaEnLista ? "removeFromList" : "addToList", this.movie);
    }
  }
};
</script>

<style scoped>
.card {
  border-radius: 10px;
  overflow: hidden;
  transition: transform 0.2s ease-in-out;
}
.card:hover {
  transform: scale(1.05);
}
.card-img-top {
  height: 230px;
  object-fit: cover;
}
</style>
