﻿import axios from 'axios';

export default {
    template: '#tpl-usersSearchFilter',
    data() {
        return {
            filter: {
                scopes: [],
                claims: [],
                createdDateTo: null,
                createdDateFrom: null,
                onlyActual: true
            },

            claimsLimit: 15,
            scopesList: [
                {
                    id: 1,
                    name: "scope 1"
                },
                {
                    id: 2,
                    name: 'scope 2'
                },
                {
                    id: 3,
                    name: 'scope 3'
                },
                {
                    id: 4,
                    name: 'scope 4'
                },
                {
                    id: 5,
                    name: 'scope 5'
                },
                {
                    id: 6,
                    name: "scope 6"
                },
                {
                    id: 7,
                    name: 'scope 7'
                },
                {
                    id: 8,
                    name: 'scope 8'
                },
                {
                    id: 9,
                    name: 'scope 9'
                },
                {
                    id: 10,
                    name: 'scope 10'
                }
            ],
            claimsList: [],
            isClaimsLoading: false
        };
    },
    methods: {
        findClaimsAsync: async function(query) {
            this.isClaimsLoading = true;

            var searchQuery = { textQuery: query, scopeIds: (this.filter.scopes === null ? [] : this.filter.scopes.map(x => x.id)), count: this.claimsLimit };
            var res = await axios.post('/api/claim/search', searchQuery);

            this.isClaimsLoading = false;

            if (res.status == 200) {
                this.claimsList = res.data.claims;
            }
            else
            {
                this.claimsList = [];
            }
        },
        getFilter: function () {
            return this.filter;
        }
    }
};