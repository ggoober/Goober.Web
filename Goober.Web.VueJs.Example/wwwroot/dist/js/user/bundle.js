(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = void 0;

var _axios = _interopRequireDefault(require("axios"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

var _default = {
  template: '#usersSearchFilter',
  data: function data() {
    return {
      filter: {
        scopes: [],
        claims: [],
        createdDateTo: null,
        createdDateFrom: null,
        onlyActual: true
      },
      claimsLimit: 15,
      scopesList: [{
        id: 1,
        name: "scope 1"
      }, {
        id: 2,
        name: 'scope 2'
      }, {
        id: 3,
        name: 'scope 3'
      }, {
        id: 4,
        name: 'scope 4'
      }, {
        id: 5,
        name: 'scope 5'
      }, {
        id: 6,
        name: "scope 6"
      }, {
        id: 7,
        name: 'scope 7'
      }, {
        id: 8,
        name: 'scope 8'
      }, {
        id: 9,
        name: 'scope 9'
      }, {
        id: 10,
        name: 'scope 10'
      }],
      claimsList: [],
      isClaimsLoading: false
    };
  },
  methods: {
    findClaimsAsync: function () {
      var _findClaimsAsync = _asyncToGenerator( /*#__PURE__*/regeneratorRuntime.mark(function _callee(query) {
        var searchQuery, res;
        return regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                this.isClaimsLoading = true;
                searchQuery = {
                  textQuery: query,
                  scopeIds: this.filter.scopes === null ? [] : this.filter.scopes.map(function (x) {
                    return x.id;
                  }),
                  count: this.claimsLimit
                };
                _context.next = 4;
                return _axios.default.post('/api/claim/search', searchQuery);

              case 4:
                res = _context.sent;
                this.isClaimsLoading = false;

                if (res.status == 200) {
                  this.claimsList = res.data.claims;
                } else {
                  this.claimsList = [];
                }

              case 7:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, this);
      }));

      function findClaimsAsync(_x) {
        return _findClaimsAsync.apply(this, arguments);
      }

      return findClaimsAsync;
    }(),
    getFilter: function getFilter() {
      return this.filter;
    }
  }
};
exports.default = _default;

},{"axios":"axios"}],2:[function(require,module,exports){
"use strict";

var _vue = _interopRequireDefault(require("vue"));

var _axios = _interopRequireDefault(require("axios"));

var _vueMultiselect = _interopRequireDefault(require("vue-multiselect"));

var _usersSearchFilter = _interopRequireDefault(require("./components/users-search-filter"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_vue.default.component('vue-multiselect', _vueMultiselect.default);

var vueApp = new _vue.default({
  el: '#users-search',
  data: function data() {
    return {
      backendUrls: globalThis.backendUrls,
      errorMessage: null,
      searchFilter: null
    };
  },
  mounted: function mounted() {
    this.$refs.searchFilter.filter.scopes = [{
      id: 3,
      name: "scope 3"
    }];
    this.searchFilter = this.$refs.searchFilter.filter;
  },
  methods: {
    findClick: function findClick() {
      var searchFilter = this.$refs.searchFilter.getFilter();
      console.log('findClick: ' + JSON.stringify(searchFilter));
      searchFilter.scopes = [{
        id: 3,
        name: "scope 3"
      }];
    }
  },
  components: {
    'users-search-filter': _usersSearchFilter.default
  }
});

},{"./components/users-search-filter":1,"axios":"axios","vue":"vue","vue-multiselect":"vue-multiselect"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXIuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9pbmRleC5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7QUNBQzs7Ozs7Ozs7ZUFFYztBQUNYLEVBQUEsUUFBUSxFQUFFLG9CQURDO0FBRVgsRUFBQSxJQUZXLGtCQUVKO0FBQ0gsV0FBTztBQUNILE1BQUEsTUFBTSxFQUFFO0FBQ0osUUFBQSxNQUFNLEVBQUUsRUFESjtBQUVKLFFBQUEsTUFBTSxFQUFFLEVBRko7QUFHSixRQUFBLGFBQWEsRUFBRSxJQUhYO0FBSUosUUFBQSxlQUFlLEVBQUUsSUFKYjtBQUtKLFFBQUEsVUFBVSxFQUFFO0FBTFIsT0FETDtBQVNILE1BQUEsV0FBVyxFQUFFLEVBVFY7QUFVSCxNQUFBLFVBQVUsRUFBRSxDQUNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FEUSxFQUtSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FMUSxFQVNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FUUSxFQWFSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FiUSxFQWlCUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BakJRLEVBcUJSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtB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RUFBRSxFQUFFLENBQU47QUFBUyxRQUFBLElBQUksRUFBRTtBQUFmLE9BQUQsQ0FBdEI7QUFDSDtBQVBMLEdBaEJpQjtBQXlCakIsRUFBQSxVQUFVLEVBQUU7QUFDUiwyQkFBdUI7QUFEZjtBQXpCSyxDQUFSLENBQWIiLCJmaWxlIjoiZ2VuZXJhdGVkLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXNDb250ZW50IjpbIihmdW5jdGlvbigpe2Z1bmN0aW9uIHIoZSxuLHQpe2Z1bmN0aW9uIG8oaSxmKXtpZighbltpXSl7aWYoIWVbaV0pe3ZhciBjPVwiZnVuY3Rpb25cIj09dHlwZW9mIHJlcXVpcmUmJnJlcXVpcmU7aWYoIWYmJmMpcmV0dXJuIGMoaSwhMCk7aWYodSlyZXR1cm4gdShpLCEwKTt2YXIgYT1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK2krXCInXCIpO3Rocm93IGEuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixhfXZhciBwPW5baV09e2V4cG9ydHM6e319O2VbaV1bMF0uY2FsbChwLmV4cG9ydHMsZnVuY3Rpb24ocil7dmFyIG49ZVtpXVsxXVtyXTtyZXR1cm4gbyhufHxyKX0scCxwLmV4cG9ydHMscixlLG4sdCl9cmV0dXJuIG5baV0uZXhwb3J0c31mb3IodmFyIHU9XCJmdW5jdGlvblwiPT10eXBlb2YgcmVxdWlyZSYmcmVxdWlyZSxpPTA7aTx0Lmxlbmd0aDtpKyspbyh0W2ldKTtyZXR1cm4gb31yZXR1cm4gcn0pKCkiLCLvu79pbXBvcnQgYXhpb3MgZnJvbSAnYXhpb3MnO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQge1xyXG4gICAgdGVtcGxhdGU6ICcjdXNlcnNTZWFyY2hGaWx0ZXInLFxyXG4gICAgZGF0YSgpIHtcclxuICAgICAgICByZXR1cm4ge1xyXG4gICAgICAgICAgICBmaWx0ZXI6IHtcclxuICAgICAgICAgICAgICAgIHNjb3BlczogW10sXHJcbiAgICAgICAgICAgICAgICBjbGFpbXM6IFtdLFxyXG4gICAgICAgICAgICAgICAgY3JlYXRlZERhdGVUbzogbnVsbCxcclxuICAgICAgICAgICAgICAgIGNyZWF0ZWREYXRlRnJvbTogbnVsbCxcclxuICAgICAgICAgICAgICAgIG9ubHlBY3R1YWw6IHRydWVcclxuICAgICAgICAgICAgfSxcclxuXHJcbiAgICAgICAgICAgIGNsYWltc0xpbWl0OiAxNSxcclxuICAgICAgICAgICAgc2NvcGVzTGlzdDogW1xyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiAxLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6IFwic2NvcGUgMVwiXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiAyLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSAyJ1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMyxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgMydcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDQsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDQnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA1LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA1J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNixcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiBcInNjb3BlIDZcIlxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNyxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgNydcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDgsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDgnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA5LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA5J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMTAsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDEwJ1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBdLFxyXG4gICAgICAgICAgICBjbGFpbXNMaXN0OiBbXSxcclxuICAgICAgICAgICAgaXNDbGFpbXNMb2FkaW5nOiBmYWxzZVxyXG4gICAgICAgIH07XHJcbiAgICB9LFxyXG4gICAgbWV0aG9kczoge1xyXG4gICAgICAgIGZpbmRDbGFpbXNBc3luYzogYXN5bmMgZnVuY3Rpb24ocXVlcnkpIHtcclxuICAgICAgICAgICAgdGhpcy5pc0NsYWltc0xvYWRpbmcgPSB0cnVlO1xyXG5cclxuICAgICAgICAgICAgdmFyIHNlYXJjaFF1ZXJ5ID0geyB0ZXh0UXVlcnk6IHF1ZXJ5LCBzY29wZUlkczogKHRoaXMuZmlsdGVyLnNjb3BlcyA9PT0gbnVsbCA/IFtdIDogdGhpcy5maWx0ZXIuc2NvcGVzLm1hcCh4ID0+IHguaWQpKSwgY291bnQ6IHRoaXMuY2xhaW1zTGltaXQgfTtcclxuICAgICAgICAgICAgdmFyIHJlcyA9IGF3YWl0IGF4aW9zLnBvc3QoJy9hcGkvY2xhaW0vc2VhcmNoJywgc2VhcmNoUXVlcnkpO1xyXG5cclxuICAgICAgICAgICAgdGhpcy5pc0NsYWltc0xvYWRpbmcgPSBmYWxzZTtcclxuXHJcbiAgICAgICAgICAgIGlmIChyZXMuc3RhdHVzID09IDIwMCkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5jbGFpbXNMaXN0ID0gcmVzLmRhdGEuY2xhaW1zO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIGVsc2VcclxuICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5jbGFpbXNMaXN0ID0gW107XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9LFxyXG4gICAgICAgIGdldEZpbHRlcjogZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICByZXR1cm4gdGhpcy5maWx0ZXI7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59OyIsIu+7v2ltcG9ydCBWdWUgZnJvbSAndnVlJztcclxuaW1wb3J0IGF4aW9zIGZyb20gJ2F4aW9zJztcclxuaW1wb3J0IE11bHRpc2VsZWN0IGZyb20gJ3Z1ZS1tdWx0aXNlbGVjdCdcclxuXHJcblZ1ZS5jb21wb25lbnQoJ3Z1ZS1tdWx0aXNlbGVjdCcsIE11bHRpc2VsZWN0KVxyXG5cclxuaW1wb3J0IHVzZXJzU2VhcmNoRmlsdGVyIGZyb20gJy4vY29tcG9uZW50cy91c2Vycy1zZWFyY2gtZmlsdGVyJztcclxuXHJcbnZhciB2dWVBcHAgPSBuZXcgVnVlKHtcclxuICAgIGVsOiAnI3VzZXJzLXNlYXJjaCcsXHJcbiAgICBkYXRhKCkge1xyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgIGJhY2tlbmRVcmxzOiBnbG9iYWxUaGlzLmJhY2tlbmRVcmxzLFxyXG4gICAgICAgICAgICBlcnJvck1lc3NhZ2U6IG51bGwsXHJcbiAgICAgICAgICAgIHNlYXJjaEZpbHRlcjogbnVsbFxyXG4gICAgICAgIH07XHJcbiAgICB9LFxyXG4gICAgbW91bnRlZDogZnVuY3Rpb24gKClcclxuICAgIHtcclxuICAgICAgICB0aGlzLiRyZWZzLnNlYXJjaEZpbHRlci5maWx0ZXIuc2NvcGVzID0gW3sgaWQ6IDMsIG5hbWU6IFwic2NvcGUgM1wiIH1dO1xyXG5cclxuICAgICAgICB0aGlzLnNlYXJjaEZpbHRlciA9IHRoaXMuJHJlZnMuc2VhcmNoRmlsdGVyLmZpbHRlcjtcclxuICAgIH0sXHJcbiAgICBtZXRob2RzOlxyXG4gICAge1xyXG4gICAgICAgIGZpbmRDbGljazogZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICB2YXIgc2VhcmNoRmlsdGVyID0gdGhpcy4kcmVmcy5zZWFyY2hGaWx0ZXIuZ2V0RmlsdGVyKCk7XHJcblxyXG4gICAgICAgICAgICBjb25zb2xlLmxvZygnZmluZENsaWNrOiAnICsgSlNPTi5zdHJpbmdpZnkoc2VhcmNoRmlsdGVyKSk7XHJcblxyXG4gICAgICAgICAgICBzZWFyY2hGaWx0ZXIuc2NvcGVzID0gW3sgaWQ6IDMsIG5hbWU6IFwic2NvcGUgM1wiIH1dO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcbiAgICBjb21wb25lbnRzOiB7XHJcbiAgICAgICAgJ3VzZXJzLXNlYXJjaC1maWx0ZXInOiB1c2Vyc1NlYXJjaEZpbHRlclxyXG4gICAgfVxyXG59KTsiXX0=
