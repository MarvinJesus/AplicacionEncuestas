var FilterController = function () {

	var removeFilter = function (e) {

		$(this).fadeOut(function () {
			$(this).remove();
			$('#search-filters-container').append(filterNotSelectedTemplate($(e.target)
                .attr('data-filter-id'), $(this).text()));
            removeFromList($(this).text());
		});
	};

	var applyFilter = function (e) {
		$(this).fadeOut(function () {
			$(this).parents("li").remove();
			$('#selected-filters').append(applyfilterTemplate($(e.target)
                .attr('data-filter-id'), $(this).text()));
            addToList($(this).text());
		});
	};

    var addToList = function (categoryName) {
        var value = $(".categoryList").val();
        var result = value.concat(categoryName, ",");
        $(".categoryList").val(result.replace(/\s/g, ""));
    };

    var removeFromList = function (categoryName) {
        var cleanValue = categoryName.replace(/\s/g, "");
        var result = $(".categoryList").val().replace(cleanValue + ",", "");
        $(".categoryList").val(result.replace(/\s/g, ""));
    };

	var applyfilterTemplate = function (id, name) {
		return `<label class="label label-default font-size-3 selected-filter js-remove-filter" data-filter-id="${id}">${name}
							<span class="badge"><i class="fas fa-times-circle"></i></span>
						</label>`;
	};

	var filterNotSelectedTemplate = function (id, name) {
		return `<li class="list-group-item">
							<a href="#" class="filter font-size-2 js-apply-filter" data-filter-id="${id}">${name}</a>
						</li>`;
	};

	var init = function (searchFiltersContainer, filterContainerApplied) {
		$(filterContainerApplied).on("click", ".js-remove-filter", removeFilter);
		$(searchFiltersContainer).on("click", ".js-apply-filter", applyFilter);
	};

	return {
		init: init
	};

}();