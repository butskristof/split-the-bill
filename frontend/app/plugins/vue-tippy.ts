import VueTippy from 'vue-tippy';
import 'tippy.js/dist/tippy.css';

export default defineNuxtPlugin((nuxt) => {
  nuxt.vueApp.use(VueTippy, {
    defaultProps: {
      theme: 'app',
      arrow: true,
    },
  });
});
