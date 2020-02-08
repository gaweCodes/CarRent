import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '@/views/Home.vue';
import Brand from '@/views/Brand.vue';
import Category from '@/views/Category.vue';
import CarModel from '@/views/CarModel.vue';
import Car from '@/views/Car.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/brand',
    name: 'brand',
    component: Brand
  },
  {
    path: '/category',
    name: 'category',
    component: Category
  },
  {
    path: '/model',
    name: 'model',
    component: CarModel
  },
  {
    path: '/car',
    name: 'car',
    component: Car
  }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
});

export default router;
