// AJAX live search using Fetch API
(function () {
    var searchInput = document.getElementById("searchInput");
    var searchResults = document.getElementById("searchResults");
    var searchResultsList = document.getElementById("searchResultsList");
    var bookGrid = document.getElementById("bookGrid");

    if (!searchInput) return;

    var debounceTimer = null;

    searchInput.addEventListener("input", function () {
        var keyword = this.value.trim();

        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(function () {
            if (keyword.length < 2) {
                searchResults.classList.add("d-none");
                bookGrid.classList.remove("d-none");
                return;
            }

            fetch("/Home/Search?keyword=" + encodeURIComponent(keyword), {
                headers: { "X-Requested-With": "XMLHttpRequest" }
            })
            .then(function (response) {
                if (!response.ok) throw new Error("Network error");
                return response.json();
            })
            .then(function (books) {
                bookGrid.classList.add("d-none");
                searchResults.classList.remove("d-none");

                if (books.length === 0) {
                    searchResultsList.innerHTML = '<div class="col-12"><p class="text-center text-muted py-3">No books found for "' + keyword + '".</p></div>';
                    return;
                }

                searchResultsList.innerHTML = books.map(function (b) {
                    var available = b.availableQuantity > 0
                        ? '<span class="badge bg-success">' + b.availableQuantity + ' Available</span>'
                        : '<span class="badge bg-danger">Unavailable</span>';

                    return '<div class="col">'
                        + '<div class="card h-100 shadow-sm">'
                        + '<img src="' + b.coverImagePath + '" class="card-img-top book-cover" onerror="this.src=\'/images/no-cover.svg\'" alt="' + b.title + '">'
                        + '<div class="card-body">'
                        + '<span class="badge bg-info text-dark mb-1">' + b.categoryName + '</span>'
                        + '<h6 class="card-title fw-bold">' + b.title + '</h6>'
                        + '<p class="card-text text-muted small">' + b.author + '</p>'
                        + available
                        + '</div>'
                        + '<div class="card-footer bg-transparent">'
                        + '<a href="/Book/Details/' + b.id + '" class="btn btn-outline-primary btn-sm w-100">View Details</a>'
                        + '</div>'
                        + '</div>'
                        + '</div>';
                }).join("");
            })
            .catch(function (err) {
                console.error("Search error:", err);
            });
        }, 350);
    });

    // Clear search and restore grid
    searchInput.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            this.value = "";
            searchResults.classList.add("d-none");
            bookGrid.classList.remove("d-none");
        }
    });
}());
