import Vue from 'vue';
import axios from 'axios';

import usersSearchFilter from './components/usersSearchFilter';

var vueApp = new Vue({
    el: '#users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
            searchFilter: null
        };
    },
    components: {
        'users-search-filter': usersSearchFilter
    },
});