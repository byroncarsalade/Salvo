var app = new Vue({
    el: '#app',
    data: {
        games: [],
        scores: [],
        email: "",
        password: "",
        modal: {
            tittle: "",
            message: ""
        },
        player: null
    },
    mounted() {
        this.getGames();
    },
    methods: {
        joinGame(gId) {
            var gpId = null;
            axios.post('/api/games/' + gId + '/players')
                .then(response => {
                    gpId = response.data;
                    window.location.href = '/game.html?gp=' + gpId;
                })
                .catch(error => {
                    alert("erro al unirse al juego");
                });
        },
        createGame() {
            var gpId = null;
            axios.post('/api/games')
                .then(response => {
                    gpId = response.data;
                    window.location.href = '/game.html?gp=' + gpId;
                })
                .catch(error => {
                    alert("erro al obtener los datos");
                });
        },
        returnGame(gpId) {
            window.location.href = '/game.html?gp=' + gpId;
        },
        getGames: function (){
            this.showLogin(false);
            axios.get('/api/games')
                .then(response => {
                    this.player = response.data.email;
                    this.games = response.data.games;
                    this.getScores(this.games)
                    if (this.player == "Guest")
                        this.showLogin(true);
                })
                .catch(error => {
                    alert("erro al obtener los datos");
                });
        },
        showModal: function (show) {
            if (show)
                $("#infoModal").modal('show');
            else
                $("#infoModal").modal('hide');
        },
        showLogin: function (show) {
            if (show) {
                $("#login-form").show();
                $("#login-form").trigger("reset");
                this.email = "";
                this.password = "";
            }
            else
                $("#login-form").hide();
        },
        logout: function () {
            axios.post('/api/auth/logout')
                .then(result => {
                    if (result.status == 200) {
                        this.showLogin(true);
                        this.getGames();
                    }
                })
                .catch(error => {
                    alert("Ocurrió un error al cerrar sesión");
                });
        },
        login: function(event){
            axios.post('/api/auth/login', {
                email: this.email, password: this.password
            })
                .then(result => {
                    if (result.status == 200) {
                        this.showLogin(false);
                        this.getGames();
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 401) {
                        this.modal.tittle = "Falló la autenticación";
                        this.modal.message = "Email o contraseña inválido"
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Fall&Oacute;la autenticaci&oacute;n";
                        this.modal.message = "Ha ocurrido un error";
                        this.showModal(true);
                    }
                });
        },
        signin: function (event) {
            axios.post('/api/players', {
                email: this.email, password: this.password
            })
                .then(result => {
                    if (result.status == 201) {
                        this.login();
                    }
                })
                .catch(error => {
                    console.log("error, código de estatus: " + error.response.status);
                    if (error.response.status == 403) {
                        this.modal.tittle = "Falló el registro";
                        this.modal.message = error.response.data
                        this.showModal(true);
                    }
                    else {
                        this.modal.tittle = "Fall&Oacute;la autenticaci&oacute;n";
                        this.modal.message = "Ha ocurrido un error";
                        this.showModal(true);
                    }
                });
        },
        getScores: function (games) {
            var scores = [];
            games.forEach(game => {
                game.gamePlayers.forEach(gp => {
                    var index = scores.findIndex(sc => sc.email == gp.player.email)
                    if (index < 0) {
                        var score = { email: gp.player.email, win: 0, tie: 0, lost: 0, total: 0 }
                        switch (gp.point) {
                            case 1:
                                score.win++;
                                break;
                            case 0:
                                score.lost++;
                                break;
                            case 0.5:
                                score.tie++;
                                break;
                        }
                        score.total += gp.point;
                        scores.push(score);
                    }
                    else {
                        switch (gp.point) {
                            case 1:
                                scores[index].win++;
                                break;
                            case 0:
                                scores[index].lost++;
                                break;
                            case 0.5:
                                scores[index].tie++;
                                break;
                        }
                        scores[index].total += gp.point;
                    }
                })
            })
            app.scores = scores;
        }
    },
    filters: {
        dateFormat(date) {
            return moment(date).format('LLL');
        }
    }
})

