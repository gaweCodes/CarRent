<template>
  <nav aria-label="page navigation">
    <ul class="pagination justify-content-center">
      <li :class="{ 'page-item': true, disabled: isPreviousDisabled }">
        <a class="page-link" @click="updatePagination(step)">
          <i class="fas fa-angle-double-left"></i>
        </a>
      </li>
      <li :class="{ 'page-item': true, disabled: isPreviousDisabled }">
        <a class="page-link" @click="updatePagination(current - step)">
          <i class="fas fa-angle-left"></i>
        </a>
      </li>
      <li
        v-for="numNav in numberedNavigators"
        :key="numNav"
        :class="{ 'page-item': true, active: numNav === current }"
      >
        <a class="page-link" @click="updatePagination(numNav)">{{ numNav }}</a>
      </li>
      <li :class="{ 'page-item': true, disabled: isNextDisabled }">
        <a class="page-link" @click="updatePagination(current + step)">
          <i class="fas fa-angle-right"></i>
        </a>
      </li>
      <li :class="{ 'page-item': true, disabled: isNextDisabled }">
        <a class="page-link" @click="updatePagination(lastPage)">
          <i class="fas fa-angle-double-right"></i>
        </a>
      </li>
    </ul>
  </nav>
</template>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  props: {
    current: { type: Number, required: true },
    step: { type: Number, required: true },
    total: { type: Number, required: true }
  },
  computed: {
    isPreviousDisabled(): boolean {
      return this.current === this.step;
    },
    isNextDisabled(): boolean {
      return this.current === this.lastPage;
    },
    numberedNavigators() {
      const firstNav =
        this.current < 4 * this.step
          ? this.step
          : this.current > this.lastPage - 4 * this.step
          ? this.lastPage - 4 * this.step
          : this.current - 2 * this.step;

      const navs = [firstNav];

      if (firstNav + this.step <= this.lastPage) {
        navs.push(firstNav + this.step);
      }
      if (firstNav + this.step * 2 <= this.lastPage) {
        navs.push(firstNav + this.step * 2);
      }
      if (firstNav + this.step * 3 <= this.lastPage) {
        navs.push(firstNav + this.step * 3);
      }
      if (firstNav + this.step * 4 <= this.lastPage) {
        navs.push(firstNav + this.step * 4);
      }

      return navs;
    },
    lastPage() {
      return Math.ceil(this.total / this.step) * this.step;
    }
  },
  methods: {
    updatePagination(val: number) {
      this.$emit('changed', val);
    }
  }
});
</script>
<style scoped>
li.page-item {
  cursor: pointer;
}
</style>
