import Vue from 'vue';
import Multiselect from 'vue-multiselect'

Vue.component('vue-multiselect', Multiselect)

import vueiUsersSearchFilter from './components/users-search-filter';

var vueApp = new Vue({
    el: '#tpl-users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
            searchFilter: null
        };
    },
    mounted: function ()
    {
        this.searchFilter = this.$refs.refUsersSearchFilter.getFilter();

        this.searchFilter.scopes = [{ id: 3, name: "scope 3" }];
    },
    methods:
    {
        findClick: function () {
            console.log('findClick: ' + JSON.stringify(this.searchFilter));

            this.searchFilter.scopes = [{ id: 3, name: "scope 3" }];
        }
    },
    components: {
        'vue-users-search-filter': vueiUsersSearchFilter
    }
});