function EditAmount(mId, mAmount, mUrl) {
    /*To edit the amount of an ingredient on the ViewCocktail partial page.*/
    if (($('#CockDescription  input').length) == 0) {
        var myId = "#" + mId;
        var myImpBox = "<input id='amount' type='text' style='width:50px' value=" + mAmount + " name='amount' >";
        $(myId).html(myImpBox);
        mIni = "<a href=\"javascript:void(0)\" onclick=\""
        mySaveAnchor = mIni + "myPost(\'" + mUrl + "\',\'" + mId + "\')\">Save</a>";
        myCancelAnchor = mIni + "myCancel(\'" + mId + "\',\'" + mAmount + "\',\'" + mUrl + "\')\">Cancel</a>"
        $(myId).next().next().next().html(mySaveAnchor + " " + myCancelAnchor);
        $('.Am').hide();
    };
};
function GetACockIng(mCock) {
    /*Get all Cocktail ingredients given a cocktailid*/
    var url = "";
    url = '/Cocktail/WatchIngredients/' + mCock;
    this.GetIt = $.get(url, null, function (html) {
        var i = 0;
        var mRowsToAdd = '';
        var id = url.substr(url.lastIndexOf("/") + 1);
        $("#CockDescription").empty('form').remove('form');
        $('#CockDescription').append(html);
    });
}
function myPost(url, mItem) {
    /* Post an amount of an ingredient to the database*/
    var myId = "#" + mItem;
    var myAmount = $("#amount").attr("value");
    var myUrl = url; /* Save url before modification to use it on "amount" link*/
    var mRep = url.substr(url.lastIndexOf("/"), url.length);
    url = url.replace(mRep, "/" + myAmount + mRep);
    //url = url + "/" + myAmount
    $.post(url, function () {
        $(myId).text(myAmount);
        $(myId).next().next().next().html("<a class=\"Am\" href=\"javascript:void(0)\" onclick=\"EditAmount(\'" + mItem + "\',\'" + myAmount + "\',\'" + myUrl + "\')\">Amount</a>");
    });
    $('.Am').show();
};
function myCancel(mItem, myAmount, url) {
    /* Cancel the change of an amount of an ingredient */

    var myId = "#" + mItem;
    $(myId).text(myAmount);
    $(myId).next().next().next().html("<a class=\"Am\" href=\"javascript:void(0)\" onclick=\"EditAmount(\'" + mItem + "\',\'" + myAmount + "\',\'" + url + "\')\">Amount</a>");
    $('.Am').show();
};
function DeleteIngredient(url, mItem) {
    /* Post an amount of an ingredient to the database*/
    var myId = "#" + mItem;


    var myUrl = url; /* Save url before modification to use it on "amount" link*/
    mCocktailId = url.substr(18, url.indexOf("/", 18) - 18);
    $.post(url, function () {
    });
    var myGetit = new GetACockIng(mCocktailId);
    myGetit.GetIt;
};

function EditCocktail(mCock) {
    /*Edit a Cocktail */
    var url = "";
    url = '/Cocktail/Edit/' + mCock;
    $.get(url, null, function (html) {
        $("#CockEdit").empty('form').remove('form');
        $('#CockEdit').append(html);
    });

};
function CreateCocktail() {
    /*Create a Cocktail */
    var url = "";
    url = '/Cocktail/Create';
    $.get(url, null, function (html) {
        $("#CockEdit").empty('form').remove('form');
        $('#CockEdit').append(html);
    });

};
function GetACock(mCock) {
    /*Get all Cocktail ingredients given a cocktailid*/
    var url = "";
    url = '/Cocktail/WatchCocktail/' + mCock;
    this.GetIt = $.get(url, null, function (html) {
        var i = 0;
        var mRowsToAdd = '';
        var id = url.substr(url.lastIndexOf("/") + 1);
        $("#CockDescription").empty('form').remove('form');
        $('#CockDescription').append(html);
    });
}