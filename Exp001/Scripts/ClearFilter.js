angular.module('ClearFilter',[]).directive('clearable', clearable);

function clearable() {
    var directive = {
        restrict: 'A',
        require: 'ngModel',
        link: link
    };
    return directive;

    function link(scope, elem, attrs, ctrl) {
        elem.addClass('clearable');

        elem.bind('input', function () {
            elem[toggleClass(elem.val())]('x');
        });

        elem.on('mousemove', function (e) {
            if (elem.hasClass('x')) {
                elem[toggleClass(this.offsetWidth - 25 < e.clientX - this.getBoundingClientRect().left)]('onX');
            }
        });

        elem.on('click', function (e) {
            if (elem.hasClass('onX')) {
                elem.removeClass('x onX').val(undefined);
                ctrl.$setViewValue(undefined);
                ctrl.$render();
                scope.$digest();
            }
        });

        function toggleClass(v) {
            return v ? 'addClass' : 'removeClass';
        }
    }
}