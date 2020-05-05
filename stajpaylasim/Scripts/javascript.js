var DatatablesDataSourceHtml = function () {
    var e = JSON.parse('[[1,"Mutfak","Tava"]]');
    return {
        init: function () {
            $("#m_table_1").DataTable({
                responsive: !0, data: e, columnDefs: [{
                    targets: -1, title: "İşlemler", orderable: !1, render: function (e, a, i, n) {
                        return '\n         <a href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="Ekle">\n                          <i class="la la-plus"></i>\n                        </a>'
                    }
                }]
            })
        }
    }
}(); jQuery(document).ready(function () { DatatablesDataSourceHtml.init() });