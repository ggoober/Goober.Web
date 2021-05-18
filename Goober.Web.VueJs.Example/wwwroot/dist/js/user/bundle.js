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
  template: '#tpl-usersSearchFilter',
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

var _vueMultiselect = _interopRequireDefault(require("vue-multiselect"));

var _usersSearchFilter = _interopRequireDefault(require("./components/users-search-filter"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_vue.default.component('vue-multiselect', _vueMultiselect.default);

var vueApp = new _vue.default({
  el: '#tpl-users-search',
  data: function data() {
    return {
      backendUrls: globalThis.backendUrls,
      errorMessage: null,
      searchFilter: null
    };
  },
  mounted: function mounted() {
    this.searchFilter = this.$refs.refUsersSearchFilter.getFilter();
    this.searchFilter.scopes = [{
      id: 3,
      name: "scope 3"
    }];
  },
  methods: {
    findClick: function findClick() {
      console.log('findClick: ' + JSON.stringify(this.searchFilter));
      this.searchFilter.scopes = [{
        id: 3,
        name: "scope 3"
      }];
    }
  },
  components: {
    'vue-users-search-filter': _usersSearchFilter.default
  }
});

},{"./components/users-search-filter":1,"vue":"vue","vue-multiselect":"vue-multiselect"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXIuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9pbmRleC5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7QUNBQzs7Ozs7Ozs7ZUFFYztBQUNYLEVBQUEsUUFBUSxFQUFFLHdCQURDO0FBRVgsRUFBQSxJQUZXLGtCQUVKO0FBQ0gsV0FBTztBQUNILE1BQUEsTUFBTSxFQUFFO0FBQ0osUUFBQSxNQUFNLEVBQUUsRUFESjtBQUVKLFFBQUEsTUFBTSxFQUFFLEVBRko7QUFHSixRQUFBLGFBQWEsRUFBRSxJQUhYO0FBSUosUUFBQSxlQUFlLEVBQUUsSUFKYjtBQUtKLFFBQUEsVUFBVSxFQUFFO0FBTFIsT0FETDtBQVNILE1BQUEsV0FBVyxFQUFFLEVBVFY7QUFVSCxNQUFBLFVBQVUsRUFBRSxDQUNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FEUSxFQUtSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FMUSxFQVNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FUUSxFQWFSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FiUSxFQWlCUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BakJRLEVBcUJSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtB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dUJqQixFQUFBLFVBQVUsRUFBRTtBQUNSLCtCQUEyQjtBQURuQjtBQXZCSyxDQUFSLENBQWIiLCJmaWxlIjoiZ2VuZXJhdGVkLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXNDb250ZW50IjpbIihmdW5jdGlvbigpe2Z1bmN0aW9uIHIoZSxuLHQpe2Z1bmN0aW9uIG8oaSxmKXtpZighbltpXSl7aWYoIWVbaV0pe3ZhciBjPVwiZnVuY3Rpb25cIj09dHlwZW9mIHJlcXVpcmUmJnJlcXVpcmU7aWYoIWYmJmMpcmV0dXJuIGMoaSwhMCk7aWYodSlyZXR1cm4gdShpLCEwKTt2YXIgYT1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK2krXCInXCIpO3Rocm93IGEuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixhfXZhciBwPW5baV09e2V4cG9ydHM6e319O2VbaV1bMF0uY2FsbChwLmV4cG9ydHMsZnVuY3Rpb24ocil7dmFyIG49ZVtpXVsxXVtyXTtyZXR1cm4gbyhufHxyKX0scCxwLmV4cG9ydHMscixlLG4sdCl9cmV0dXJuIG5baV0uZXhwb3J0c31mb3IodmFyIHU9XCJmdW5jdGlvblwiPT10eXBlb2YgcmVxdWlyZSYmcmVxdWlyZSxpPTA7aTx0Lmxlbmd0aDtpKyspbyh0W2ldKTtyZXR1cm4gb31yZXR1cm4gcn0pKCkiLCLvu79pbXBvcnQgYXhpb3MgZnJvbSAnYXhpb3MnO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQge1xyXG4gICAgdGVtcGxhdGU6ICcjdHBsLXVzZXJzU2VhcmNoRmlsdGVyJyxcclxuICAgIGRhdGEoKSB7XHJcbiAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgICAgZmlsdGVyOiB7XHJcbiAgICAgICAgICAgICAgICBzY29wZXM6IFtdLFxyXG4gICAgICAgICAgICAgICAgY2xhaW1zOiBbXSxcclxuICAgICAgICAgICAgICAgIGNyZWF0ZWREYXRlVG86IG51bGwsXHJcbiAgICAgICAgICAgICAgICBjcmVhdGVkRGF0ZUZyb206IG51bGwsXHJcbiAgICAgICAgICAgICAgICBvbmx5QWN0dWFsOiB0cnVlXHJcbiAgICAgICAgICAgIH0sXHJcblxyXG4gICAgICAgICAgICBjbGFpbXNMaW1pdDogMTUsXHJcbiAgICAgICAgICAgIHNjb3Blc0xpc3Q6IFtcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiBcInNjb3BlIDFcIlxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMixcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgMidcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDMsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDMnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA0LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA0J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgNSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDYsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogXCJzY29wZSA2XCJcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDcsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDcnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA4LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA4J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogOSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgOSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDEwLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSAxMCdcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgXSxcclxuICAgICAgICAgICAgY2xhaW1zTGlzdDogW10sXHJcbiAgICAgICAgICAgIGlzQ2xhaW1zTG9hZGluZzogZmFsc2VcclxuICAgICAgICB9O1xyXG4gICAgfSxcclxuICAgIG1ldGhvZHM6IHtcclxuICAgICAgICBmaW5kQ2xhaW1zQXN5bmM6IGFzeW5jIGZ1bmN0aW9uKHF1ZXJ5KSB7XHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgICAgIHZhciBzZWFyY2hRdWVyeSA9IHsgdGV4dFF1ZXJ5OiBxdWVyeSwgc2NvcGVJZHM6ICh0aGlzLmZpbHRlci5zY29wZXMgPT09IG51bGwgPyBbXSA6IHRoaXMuZmlsdGVyLnNjb3Blcy5tYXAoeCA9PiB4LmlkKSksIGNvdW50OiB0aGlzLmNsYWltc0xpbWl0IH07XHJcbiAgICAgICAgICAgIHZhciByZXMgPSBhd2FpdCBheGlvcy5wb3N0KCcvYXBpL2NsYWltL3NlYXJjaCcsIHNlYXJjaFF1ZXJ5KTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gICAgICAgICAgICBpZiAocmVzLnN0YXR1cyA9PSAyMDApIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IHJlcy5kYXRhLmNsYWltcztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IFtdO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSxcclxuICAgICAgICBnZXRGaWx0ZXI6IGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHRoaXMuZmlsdGVyO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufTsiLCLvu79pbXBvcnQgVnVlIGZyb20gJ3Z1ZSc7XHJcbmltcG9ydCBNdWx0aXNlbGVjdCBmcm9tICd2dWUtbXVsdGlzZWxlY3QnXHJcblxyXG5WdWUuY29tcG9uZW50KCd2dWUtbXVsdGlzZWxlY3QnLCBNdWx0aXNlbGVjdClcclxuXHJcbmltcG9ydCB2dWVpVXNlcnNTZWFyY2hGaWx0ZXIgZnJvbSAnLi9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXInO1xyXG5cclxudmFyIHZ1ZUFwcCA9IG5ldyBWdWUoe1xyXG4gICAgZWw6ICcjdHBsLXVzZXJzLXNlYXJjaCcsXHJcbiAgICBkYXRhKCkge1xyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgIGJhY2tlbmRVcmxzOiBnbG9iYWxUaGlzLmJhY2tlbmRVcmxzLFxyXG4gICAgICAgICAgICBlcnJvck1lc3NhZ2U6IG51bGwsXHJcbiAgICAgICAgICAgIHNlYXJjaEZpbHRlcjogbnVsbFxyXG4gICAgICAgIH07XHJcbiAgICB9LFxyXG4gICAgbW91bnRlZDogZnVuY3Rpb24gKClcclxuICAgIHtcclxuICAgICAgICB0aGlzLnNlYXJjaEZpbHRlciA9IHRoaXMuJHJlZnMucmVmVXNlcnNTZWFyY2hGaWx0ZXIuZ2V0RmlsdGVyKCk7XHJcblxyXG4gICAgICAgIHRoaXMuc2VhcmNoRmlsdGVyLnNjb3BlcyA9IFt7IGlkOiAzLCBuYW1lOiBcInNjb3BlIDNcIiB9XTtcclxuICAgIH0sXHJcbiAgICBtZXRob2RzOlxyXG4gICAge1xyXG4gICAgICAgIGZpbmRDbGljazogZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICBjb25zb2xlLmxvZygnZmluZENsaWNrOiAnICsgSlNPTi5zdHJpbmdpZnkodGhpcy5zZWFyY2hGaWx0ZXIpKTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuc2VhcmNoRmlsdGVyLnNjb3BlcyA9IFt7IGlkOiAzLCBuYW1lOiBcInNjb3BlIDNcIiB9XTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG4gICAgY29tcG9uZW50czoge1xyXG4gICAgICAgICd2dWUtdXNlcnMtc2VhcmNoLWZpbHRlcic6IHZ1ZWlVc2Vyc1NlYXJjaEZpbHRlclxyXG4gICAgfVxyXG59KTsiXX0=
