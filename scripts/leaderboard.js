(function ($) {
    var Leaderboard = function (elemId, options) {
        _this = this;

        // Set the options so that we can access them.
        this.config = options;

        // Grab the element from the DOM
        this.$elem = $(elemId);

        // Indicates what game session we are displaying.
        this.gameIndex = 0;

        // Inidcate whether or not the leaderboard was loaded the first time.
        this.initializeComplete = false;

        initializeLeaderboard();


        /***** LEADER BOARD FUNCTIONS *****/

        // Calls to the server to grab the latest data.
        // TODO: Gotta add in the pick3 stuff somehow. Maybe on the
        // server side.
        function playerRank() {

            // AJAX call to the model. We need to make a model that will
            // Do all the picks instead of just pick 8.
            $.post('leaderredirect.aspx',
				{},
				function (data) {
				    playerRankCallback(data);
				}, 'json');
        }

        // Callback handling the data after it comes back from the server.
        function playerRankCallback(data) {
            _this.data = data;

            if (typeof _this.data.tickerString !== "undefined") {
                $("#the-ticker").html(_this.data.tickerString);
            }


            if (!_this.initializeComplete) {
                loadLeaderboard();
                finishInitialize();
            }
        }

        function finishInitialize() {
            _this.GameChangeInterval = setInterval((function () {
                loadLeaderboard();
            }), _this.config.frequency * 1000);

            _this.initializeComplete = true;
        }

        // Sets up the initial leaderboard stuff.
        function initializeLeaderboard() {

            // Start the interval that will grab the new player data.
            _this.DataInterval = setInterval((function () {
                //_this.callback(_this.processData());
                playerRank();
            }), 300 * 1000);

            // Add the list container
            _this.$content = $('<ul>');
            _this.$elem.append(_this.$content);

            // Start the data grab!
            playerRank();
        }

        // This is called when the leaderboard needs to be loaded or data needs to switch.
        function loadLeaderboard() {

            _this.$content.empty();

            if (_this.initializeComplete) {
                _this.gameIndex++;

                if (_this.gameIndex >= _this.data.sessions.length) {
                    _this.gameIndex = 0;
                }
            }

            var session = _this.data.sessions[_this.gameIndex];
            console.log(session);
            var teamScores = session.teamScores;

            // Check if the session hase been paid yet.
            if (parseInt(session.paid) == 0) {
                var prizeList = createPrizeLookup(session);
            }

            var startTime = new Date(session.startTime);
            var endTime = new Date(session.endTime);
            $("#main-prize-amount-field").html(parseFloat(session.totalPrizePool).toLocaleString("en-US", { style: 'currency', currency: 'USD' }));
            $("#secondary-game-type-field").html(session.name + " - " + (startTime.getMonth() + 1) + "/" + startTime.getDate() + "-" + (endTime.getMonth() + 1) + "/" + endTime.getDate());

            // Check if session is pick8 or pick3
            if (typeof session.payoutNFLPick8ID !== "undefined" && session.payoutNFLPick8ID != null) {
                if (session.gameTypeName == "Standard") {
                    $("#main-game-type-field").attr("src", "../images/CONTEST LOGO-NFL 5 SELECT8.png");
                }
                else if (session.gameTypeName == "WinnerTakeAll") {
                    $("#main-game-type-field").attr("src", "../images/10 SEL 8 WINNER TAKE ALL LOGO.png");
                }
            }
            else if (typeof session.payoutNFLPick3ID !== "undefined" && session.payoutNFLPick3ID != null) {
                if (session.gameTypeName == "Standard") {
                    $("#main-game-type-field").attr("src", "../images/CONTEST LOGO-NFL 5 SELECT3.png");
                }
                else if (session.gameTypeName == "DoubleUp") {
                    $("#main-game-type-field").attr("src", "../images/SELECT 3 DOUBLE UP.png");
                }
                else if (session.gameTypeName == "Funday") {
                    $("#main-game-type-field").attr("src", "../images/SELECT 3 SUNDAY.png");
                }
            }
            else // This is PGA.
            {
                $("#main-game-type-field").attr("src", "../images/LEADERBOARD-PGA SELECT 4 LOGO.png");
            }

            if (parseInt(session.paid) == 1) {
                $(".prize-header").html("FINAL PRIZE");
                $("#leaderboard-title-art").attr("src", "../images/FINAL RESULTS.png");
            }
            else {
                $(".prize-header").html("CURRENT PRIZE");
                $("#leaderboard-title-art").attr("src", "../images/LEADERBOARD TITLE.png");
            }


            if (teamScores != null)
                for (var i = 0; i < teamScores.length && i < 100; i++) {
                    var teamScore = teamScores[i];
                    var item = $('<li>').appendTo(_this.$content);

                    var place = $('<span class="place">' + "#" + (i + 1) + '</span>').appendTo(item);
                    var username = $('<span class="name">' + teamScore.username + '</span>').appendTo(item);

                    var entryID = $('<span class="entry"></span>').appendTo(item);

                    if (typeof teamScore.userNFLPick8ID !== "undefined" && teamScore.userNFLPick8ID != null) {
                        entryID.text(calcMD5(teamScore.userNFLPick8ID).slice(-4));
                    }
                    else if (typeof teamScore.userNFLPick3ID !== "undefined" && teamScore.userNFLPick3ID != null) {
                        entryID.text(calcMD5(teamScore.userNFLPick3ID).slice(-4));
                    }
                    else {
                        entryID.text(calcMD5(teamScore.userPGAPick4ID).slice(-4));
                    }


                    var venueName = $('<span class="location">' + teamScore.venueName + '</span>').appendTo(item);


                    var prize = $('<span class="prize"></span>').appendTo(item);

                    if (parseInt(session.paid) == 1) {
                        var totPrize = parseFloat(teamScore.winAmount);

                        // TODO: Replace the something here!
                        prize.text(totPrize.toLocaleString("en-US", { style: 'currency', currency: 'USD' }));
                    }
                    else {
                        var totPrize = prizeList[i];
                        prize.text(totPrize.toLocaleString("en-US", { style: 'currency', currency: 'USD' }));
                    }


                    // Clear place and prize if there is no prize.
                    if (totPrize <= 0 || totPrize == null || totPrize == "NaN" || isNaN(totPrize)) {
                        place.text("");
                        prize.text("");
                    }

                    var points = parseFloat(teamScore.totPoints);
                    var totPoints = $('<span class="count">' + points.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + '</span>')
                                        .appendTo(item);

                    // clear place, prize, and points if there are no points.
                    if (points <= 0) {
                        place.text("");
                        prize.text("");
                        totPoints.text("");
                    }
                }
        }

        function createPrizeLookup(session) {
            var prizeList = [];

            prizeList[0] = parseFloat(session.first);
            prizeList[1] = parseFloat(session.second);
            prizeList[2] = parseFloat(session.third);
            prizeList[3] = parseFloat(session.fourth);
            prizeList[4] = parseFloat(session.fifth);
            prizeList[5] = parseFloat(session.sixth);

            prizeList[6] = parseFloat(session.seventh);
            prizeList[7] = parseFloat(session.seventh);

            prizeList[8] = parseFloat(session.eighth);
            prizeList[9] = parseFloat(session.eighth);

            for (var i = 10; i <= 14; i++) {
                prizeList[i] = parseFloat(session.ninth);
            }

            for (var i = 15; i <= 19; i++) {
                prizeList[i] = parseFloat(session.tenth);
            }

            for (var i = 20; i <= 29; i++) {
                prizeList[i] = parseFloat(session.eleventh);
            }

            for (var i = 30; i <= 44; i++) {
                prizeList[i] = parseFloat(session.twelfth);
            }

            for (var i = 45; i <= 74; i++) {
                prizeList[i] = parseFloat(session.thirteenth);
            }

            for (var i = 75; i <= 99; i++) {
                prizeList[i] = parseFloat(session.fourteenth);
            }

            return prizeList;
        }
    };

    window.Leaderboard = Leaderboard;
})(jQuery);

// Gets the leaderboard initialized.
$(document).ready(function ($) {
    var myLeaderboard = new Leaderboard(".content", { limit: 100, frequency: 140 });
});
