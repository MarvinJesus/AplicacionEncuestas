var FilterController = function () {

    var FiltersContainer;
    var FilterContainerApplied;
    var Input;

    var removeFilter = function (e) {
        $(this).fadeOut(function () {
            $(this).remove();
            $(FiltersContainer).append(filterNotSelectedTemplate($(e.target).attr('data-filter-id'), this.innerText));
            removeFromList($(this).text());
        });
    };
    var applyFilter = function (e) {
        $(this).fadeOut(function () {
            $(this).parents("li").remove();
            $(FilterContainerApplied).append(applyfilterTemplate($(e.target).attr('data-filter-id'), this.innerText));
            addToList($(this).text());
        });
    }
    var applyfilterTemplate = function (id, name) {
        return `<label class="label label-default font-size-3 selected-filter js-remove-filter" data-filter-id="${id}">${name}
                            <span class="badge"><i class="fas fa-times-circle"></i></span>
                        </label>`;
    };
    var addToList = function (categoryName) {
        var value = $(Input).val();
        var result = value.concat(categoryName, ",");
        $(Input).val(result.replace(/\s/g, ""));
    };
    var removeFromList = function (categoryName) {
        var cleanValue = categoryName.replace(/\s/g, "");
        var result = $(Input).val().replace(cleanValue + ",", "");
        $(Input).val(result.replace(/\s/g, ""));
    };
    var filterNotSelectedTemplate = function (id, name) {
        return `<li class="list-group-item">
                        <a href="#" class="filter font-size-2 js-apply-filter" data-filter-id="${id}">${name}</a>
                    </li>`;
    };
    var categoriesId = function () {
        var categories = $(FilterContainerApplied).children();
        var categoriesId = "";

        if (categories.length == 0) return null;

        categoriesId += $(categories[0]).attr("data-filter-id");
        for (var i = 1; i < categories.length; i++) {
            categoriesId += "," + $(categories[i]).attr("data-filter-id");
        }
        return categoriesId;
    };
    var init = function (filtersContainer, filterContainerApplied, input) {

        FiltersContainer = filtersContainer;
        FilterContainerApplied = filterContainerApplied;
        Input = input;

        $(filterContainerApplied).on("click", ".js-remove-filter", removeFilter);
        $(filtersContainer).on("click", ".js-apply-filter", applyFilter);
    };
    return {
        init: init,
        getCategoriesId: categoriesId
    };
}();