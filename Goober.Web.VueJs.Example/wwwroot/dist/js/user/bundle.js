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
      errorMessage: null
    };
  },
  mounted: function mounted() {
    this.$refs.searchFilter.filter.scopes = [{
      id: 3,
      name: "scope 3"
    }];
  },
  methods: {
    findClick: function findClick() {
      var searchFilter = this.$refs.searchFilter.getFilter();
      console.log('findClick: ' + JSON.stringify(searchFilter));
    }
  },
  components: {
    'users-search-filter': _usersSearchFilter.default
  }
});

},{"./components/users-search-filter":1,"axios":"axios","vue":"vue","vue-multiselect":"vue-multiselect"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXIuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9pbmRleC5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7QUNBQzs7Ozs7Ozs7ZUFFYztBQUNYLEVBQUEsUUFBUSxFQUFFLG9CQURDO0FBRVgsRUFBQSxJQUZXLGtCQUVKO0FBQ0gsV0FBTztBQUNILE1BQUEsTUFBTSxFQUFFO0FBQ0osUUFBQSxNQUFNLEVBQUUsRUFESjtBQUVKLFFBQUEsTUFBTSxFQUFFLEVBRko7QUFHSixRQUFBLGFBQWEsRUFBRSxJQUhYO0FBSUosUUFBQSxlQUFlLEVBQUUsSUFKYjtBQUtKLFFBQUEsVUFBVSxFQUFFO0FBTFIsT0FETDtBQVNILE1BQUEsV0FBVyxFQUFFLEVBVFY7QUFVSCxNQUFBLFVBQVUsRUFBRSxDQUNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FEUSxFQUtSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FMUSxFQVNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FUUSxFQWFSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FiUSxFQWlCUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BakJRLEVBcUJSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtB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dCI6WyIoZnVuY3Rpb24oKXtmdW5jdGlvbiByKGUsbix0KXtmdW5jdGlvbiBvKGksZil7aWYoIW5baV0pe2lmKCFlW2ldKXt2YXIgYz1cImZ1bmN0aW9uXCI9PXR5cGVvZiByZXF1aXJlJiZyZXF1aXJlO2lmKCFmJiZjKXJldHVybiBjKGksITApO2lmKHUpcmV0dXJuIHUoaSwhMCk7dmFyIGE9bmV3IEVycm9yKFwiQ2Fubm90IGZpbmQgbW9kdWxlICdcIitpK1wiJ1wiKTt0aHJvdyBhLmNvZGU9XCJNT0RVTEVfTk9UX0ZPVU5EXCIsYX12YXIgcD1uW2ldPXtleHBvcnRzOnt9fTtlW2ldWzBdLmNhbGwocC5leHBvcnRzLGZ1bmN0aW9uKHIpe3ZhciBuPWVbaV1bMV1bcl07cmV0dXJuIG8obnx8cil9LHAscC5leHBvcnRzLHIsZSxuLHQpfXJldHVybiBuW2ldLmV4cG9ydHN9Zm9yKHZhciB1PVwiZnVuY3Rpb25cIj09dHlwZW9mIHJlcXVpcmUmJnJlcXVpcmUsaT0wO2k8dC5sZW5ndGg7aSsrKW8odFtpXSk7cmV0dXJuIG99cmV0dXJuIHJ9KSgpIiwi77u/aW1wb3J0IGF4aW9zIGZyb20gJ2F4aW9zJztcclxuXHJcbmV4cG9ydCBkZWZhdWx0IHtcclxuICAgIHRlbXBsYXRlOiAnI3VzZXJzU2VhcmNoRmlsdGVyJyxcclxuICAgIGRhdGEoKSB7XHJcbiAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgICAgZmlsdGVyOiB7XHJcbiAgICAgICAgICAgICAgICBzY29wZXM6IFtdLFxyXG4gICAgICAgICAgICAgICAgY2xhaW1zOiBbXSxcclxuICAgICAgICAgICAgICAgIGNyZWF0ZWREYXRlVG86IG51bGwsXHJcbiAgICAgICAgICAgICAgICBjcmVhdGVkRGF0ZUZyb206IG51bGwsXHJcbiAgICAgICAgICAgICAgICBvbmx5QWN0dWFsOiB0cnVlXHJcbiAgICAgICAgICAgIH0sXHJcblxyXG4gICAgICAgICAgICBjbGFpbXNMaW1pdDogMTUsXHJcbiAgICAgICAgICAgIHNjb3Blc0xpc3Q6IFtcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiBcInNjb3BlIDFcIlxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMixcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgMidcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDMsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDMnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA0LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA0J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgNSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDYsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogXCJzY29wZSA2XCJcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDcsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDcnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA4LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA4J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogOSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgOSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDEwLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSAxMCdcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgXSxcclxuICAgICAgICAgICAgY2xhaW1zTGlzdDogW10sXHJcbiAgICAgICAgICAgIGlzQ2xhaW1zTG9hZGluZzogZmFsc2VcclxuICAgICAgICB9O1xyXG4gICAgfSxcclxuICAgIG1ldGhvZHM6IHtcclxuICAgICAgICBmaW5kQ2xhaW1zQXN5bmM6IGFzeW5jIGZ1bmN0aW9uKHF1ZXJ5KSB7XHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgICAgIHZhciBzZWFyY2hRdWVyeSA9IHsgdGV4dFF1ZXJ5OiBxdWVyeSwgc2NvcGVJZHM6ICh0aGlzLmZpbHRlci5zY29wZXMgPT09IG51bGwgPyBbXSA6IHRoaXMuZmlsdGVyLnNjb3Blcy5tYXAoeCA9PiB4LmlkKSksIGNvdW50OiB0aGlzLmNsYWltc0xpbWl0IH07XHJcbiAgICAgICAgICAgIHZhciByZXMgPSBhd2FpdCBheGlvcy5wb3N0KCcvYXBpL2NsYWltL3NlYXJjaCcsIHNlYXJjaFF1ZXJ5KTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gICAgICAgICAgICBpZiAocmVzLnN0YXR1cyA9PSAyMDApIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IHJlcy5kYXRhLmNsYWltcztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IFtdO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSxcclxuICAgICAgICBnZXRGaWx0ZXI6IGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHRoaXMuZmlsdGVyO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufTsiLCLvu79pbXBvcnQgVnVlIGZyb20gJ3Z1ZSc7XHJcbmltcG9ydCBheGlvcyBmcm9tICdheGlvcyc7XHJcbmltcG9ydCBNdWx0aXNlbGVjdCBmcm9tICd2dWUtbXVsdGlzZWxlY3QnXHJcblxyXG5WdWUuY29tcG9uZW50KCd2dWUtbXVsdGlzZWxlY3QnLCBNdWx0aXNlbGVjdClcclxuXHJcbmltcG9ydCB1c2Vyc1NlYXJjaEZpbHRlciBmcm9tICcuL2NvbXBvbmVudHMvdXNlcnMtc2VhcmNoLWZpbHRlcic7XHJcblxyXG52YXIgdnVlQXBwID0gbmV3IFZ1ZSh7XHJcbiAgICBlbDogJyN1c2Vycy1zZWFyY2gnLFxyXG4gICAgZGF0YSgpIHtcclxuICAgICAgICByZXR1cm4ge1xyXG4gICAgICAgICAgICBiYWNrZW5kVXJsczogZ2xvYmFsVGhpcy5iYWNrZW5kVXJscyxcclxuICAgICAgICAgICAgZXJyb3JNZXNzYWdlOiBudWxsLFxyXG4gICAgICAgIH07XHJcbiAgICB9LFxyXG4gICAgbW91bnRlZDogZnVuY3Rpb24gKClcclxuICAgIHtcclxuICAgICAgICB0aGlzLiRyZWZzLnNlYXJjaEZpbHRlci5maWx0ZXIuc2NvcGVzID0gW3sgaWQ6IDMsIG5hbWU6IFwic2NvcGUgM1wiIH1dO1xyXG4gICAgfSxcclxuICAgIG1ldGhvZHM6XHJcbiAgICB7XHJcbiAgICAgICAgZmluZENsaWNrOiBmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIHZhciBzZWFyY2hGaWx0ZXIgPSB0aGlzLiRyZWZzLnNlYXJjaEZpbHRlci5nZXRGaWx0ZXIoKTtcclxuXHJcbiAgICAgICAgICAgIGNvbnNvbGUubG9nKCdmaW5kQ2xpY2s6ICcgKyBKU09OLnN0cmluZ2lmeShzZWFyY2hGaWx0ZXIpKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG4gICAgY29tcG9uZW50czoge1xyXG4gICAgICAgICd1c2Vycy1zZWFyY2gtZmlsdGVyJzogdXNlcnNTZWFyY2hGaWx0ZXJcclxuICAgIH1cclxufSk7Il19
