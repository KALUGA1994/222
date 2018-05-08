angular.module('AppService', []).service('Get', function ($http) {
    var service = {}
    service.GetAdres = function (pagingInfo) {
        return $http.get('/Getter/Get', { params: pagingInfo });
    };
    return service;
})