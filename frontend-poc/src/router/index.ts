import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '@/pages/main-page/MainPage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: MainPage,
    },
    {
      path: '/groups',
      children: [
        {
          path: ':id',
          name: 'GroupDetail',
          component: () => import('@/pages/group-detail/GroupDetail.vue'),
        },
      ],
    },
  ],
});

export default router;
