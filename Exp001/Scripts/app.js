var app = angular.module('app', ['ui.bootstrap','AppService','ClearFilter']);
app.controller('AdrGet', function ($scope, Get) {
    $scope.pagingInfo = {
        itemsPerPage: "10",
        sortBy: null,
        reverse: false,
        fieldname: '',
        totalItems: 0,
        currentPage: 1,
        firstdate: null,
        seconddate: null,
        filtervalue: null
    };

    $('input[name="daterange"]').daterangepicker(
        {
            locale:
                {
                    format: 'DD.MM.YYYY',
                    cancelLabel: 'Clear'
                },
        }, function (start, end) {
            $scope.pagingInfo.firstdate = start.format('DD.MM.YYYY');
            $scope.pagingInfo.seconddate = end.format('DD.MM.YYYY');
        });

    $('input[name="daterange"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $scope.pagingInfo.firstdate = null;
        $scope.pagingInfo.seconddate = null;
        get();
    });

    $scope.someFilterWithDate = function (fieldname) {
        $scope.pagingInfo.fieldname = fieldname;
        get();
    }

    $scope.someFilter = function (fieldname, value) {
        $scope.pagingInfo.fieldname = fieldname;
        $scope.pagingInfo.filtervalue = value;
        get();
    }

    $scope.resizeItemsPerPage = function (itemsPerPage) {
        $scope.pagingInfo.itemsPerPage = itemsPerPage;
        get();
    }
    $scope.$watch("pagingInfo.itemsPerPage", function () {
        $scope.resizeItemsPerPage($scope.pagingInfo.itemsPerPage);
    });
    $scope.reverse = false;
    $scope.sort = function (sortBy) {
        if (sortBy === $scope.pagingInfo.sortBy) {
            $scope.pagingInfo.reverse = !$scope.pagingInfo.reverse;
        } else {
            $scope.pagingInfo.sortBy = sortBy;
            $scope.pagingInfo.reverse = false;
        }
        get();
    }

    $scope.isSortUp = function (sortBy) {
        return $scope.pagingInfo.sortBy === sortBy && !$scope.pagingInfo.reverse;
    };

    $scope.isSortDown = function (sortBy) {
        return $scope.pagingInfo.sortBy === sortBy && $scope.pagingInfo.reverse;
    };

    $scope.selectPage = function (page) {
        $scope.pagingInfo.page = page;
        get();
    };
    $scope.$watch("currentPage", function () {
        $scope.selectPage($scope.currentPage);
    });

    function get() {
        Get.GetAdres($scope.pagingInfo).then(function (data) {
            angular.forEach(data.data.data, function (a, key) {
                data.data.data[key].Data = moment(data.data.data[key].Data).format('DD.MM.YYYY');
            });
            $scope.adr = data.data;
            $scope.pagingInfo.totalItems = data.data.count;
        });
    }
})





