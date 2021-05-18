import Vue from 'vue';
import axios from 'axios';
import Multiselect from 'vue-multiselect'

Vue.component('vue-multiselect', Multiselect)

import usersSearchFilter from './components/users-search-filter';

var vueApp = new Vue({
    el: '#users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
        };
    },
    components: {
        'users-search-filter': usersSearchFilter
    }
});