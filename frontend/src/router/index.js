import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/LoginView.vue';
import ListasView from '../views/ListasView.vue';

const routes = [
    { path: '/', component: LoginView },
    { path: '/listas', component: ListasView }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('token');

    if (to.path === '/listas' && !token) {
        next('/'); // Redirige al login si no está autenticado
    } else {
        next(); // Permite continuar
    }
});

export default router;
