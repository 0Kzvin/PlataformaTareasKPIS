import { defineBoot } from '#q-app/wrappers';
import VueSweetAlert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';

export default defineBoot(({ app }) => {
    app.use(VueSweetAlert2);
});
