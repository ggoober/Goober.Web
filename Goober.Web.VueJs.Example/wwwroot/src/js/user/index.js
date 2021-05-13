import Vue from 'vue';
import axios from 'axios';

import usersSearchFilter from './components/users-search-filter';

var vueApp = new Vue({
    el: '#users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
            searchFilter: {}
        };
    },
    components: {
        'users-search-filter': usersSearchFilter
    },
});