$(".searchButton").click(function (e) { 
    e.preventDefault();
    
    var searchString = $(".searchTerm").val();

    //console.log(searchString);

    if (searchString.length > 2) {
        getFranchises(searchString);
    } else {
        getFranchises();
    }
});