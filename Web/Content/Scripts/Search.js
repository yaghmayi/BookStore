function Search()
{
    var searchTerm = document.getElementById("searchTextBox").value;
	if (searchTerm == null || searchTerm.trim() == "")
		window.location = "/Book/ListBooks";
	else
		window.location = "/Book/ListBooks?searchTerm=" + searchTerm;
}
