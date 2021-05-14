﻿import axios from 'axios';

export default {
    template: '#usersSearchFilter',
    props: {
        createdDateTo: null,
        createdDateFrom: null,
        errorMessage: null,
        scopes: [],
        claims: [],
        onlyActual: true
    },
    data() {
        return {
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
            isClaimsLoading: false,
            claimsList: []
        };
    },
    methods: {
        async findClaimsAsync(query) {
            this.isClaimsLoading = true;

            var res = await axios.post('/api/claim/search', { textQuery: query, scopeIds: [], count: 15 });

            this.isClaimsLoading = false;

            if (res.status == 200)
            {
                this.claimsList = res.data.claims;
            }

        }
    }
};