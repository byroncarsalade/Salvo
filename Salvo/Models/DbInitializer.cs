using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class DbInitializer
    {
        public static void Initialize(SalvoContext context)
        {

            if (!context.Players.Any())
            {
                var players = new Player[]
                {
                new Player{Email="j.bauer@ctu.gov"},
                new Player{Email="c.obrian@ctu.gov"},
                new Player{Email="kim.bauer@ctu.gov"},
                new Player{Email="t.almeida@ctu.gov"}
                };
                foreach (Player p in players)
                {
                    context.Players.Add(p);
                }
                context.SaveChanges();
            }

            if (!context.Games.Any())
            {
                var games = new Game[]
                {
                    new Game{CreationDate=DateTime.Now},
                    new Game{CreationDate=DateTime.Now.AddHours(1)},
                    new Game{CreationDate=DateTime.Now.AddHours(2)},
                    new Game{CreationDate=DateTime.Now.AddHours(3)}
                };

                foreach (Game g in games)
                {
                    context.Games.Add(g);
                }

                context.SaveChanges();
            }



            if (!context.GamesPlayers.Any())
            {
                Game game1 = context.Games.Find(1L);
                Game game2 = context.Games.Find(2L);
                Game game3 = context.Games.Find(3L);
                Game game4 = context.Games.Find(4L);

                Player jbauer = context.Players.Find(1L);
                Player obrian = context.Players.Find(2L);
                Player kbauer = context.Players.Find(3L);
                Player almeida = context.Players.Find(4L);

                var gamesPlayers = new GamePlayer[]
                {
                    new GamePlayer{JoinDate=DateTime.Now, Game = game1, Player = jbauer},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game1, Player = obrian},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game2, Player = jbauer},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game2, Player = obrian},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game3, Player = obrian},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game3, Player = almeida},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game4, Player = jbauer},
                    new GamePlayer{JoinDate=DateTime.Now, Game = game4, Player = almeida},
                };

                foreach (GamePlayer gp in gamesPlayers)
                {
                    context.GamesPlayers.Add(gp);
                }

                context.SaveChanges();
            }


            if (!context.Ships.Any())
            {
                GamePlayer gp1 = context.GamesPlayers.Find(1L);
                GamePlayer gp2 = context.GamesPlayers.Find(2L);
                GamePlayer gp3 = context.GamesPlayers.Find(3L);
                GamePlayer gp4 = context.GamesPlayers.Find(4L);
                GamePlayer gp5 = context.GamesPlayers.Find(5L);
                GamePlayer gp6 = context.GamesPlayers.Find(6L);
                GamePlayer gp7 = context.GamesPlayers.Find(7L);
                GamePlayer gp8 = context.GamesPlayers.Find(8L);

                var ships = new Ship[]
                {
                    new Ship{ Type="Destroyer", GamePlayer=gp1, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "H2"},
                        new ShipLocation { Location= "H3"},
                        new ShipLocation { Location= "H4"}
                        },
                    },
                    new Ship{ Type="Submarine", GamePlayer=gp1, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "E1"},
                        new ShipLocation { Location= "F1"},
                        new ShipLocation { Location= "G1"}
                        },
                    },
                    new Ship{ Type="PatroalBoat", GamePlayer=gp1, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "B4"},
                        new ShipLocation { Location= "B5"}
                        },
                    },
                    new Ship{ Type="Destroyer", GamePlayer=gp2, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "B5"},
                        new ShipLocation { Location= "C5"},
                        new ShipLocation { Location= "D5"}
                        },
                    },
                    new Ship{ Type="PatroalBoat", GamePlayer=gp2, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "F1"},
                        new ShipLocation { Location= "F2"}
                        },
                    },
                    new Ship{ Type="Destroyer", GamePlayer=gp3, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "B5"},
                        new ShipLocation { Location= "C5"},
                        new ShipLocation { Location= "D5"}
                        },
                    },
                    new Ship{ Type="PatroalBoat", GamePlayer=gp3, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "C6"},
                        new ShipLocation { Location= "C7"}
                        },
                    },
                    new Ship{ Type="Submarine", GamePlayer=gp4, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "A2"},
                        new ShipLocation { Location= "A3"},
                        new ShipLocation { Location= "A4"}
                        },
                    },
                    new Ship{ Type="PatroalBoat", GamePlayer=gp4, Locations= new ShipLocation[]{
                        new ShipLocation { Location= "G6"},
                        new ShipLocation { Location= "H6"}
                        },
                    },
                };

                foreach (Ship sh in ships)
                {
                    context.Ships.Add(sh);
                }

                context.SaveChanges();
            }

            if (!context.Salvos.Any())
            {
                GamePlayer gp1 = context.GamesPlayers.Find(1L);
                GamePlayer gp2 = context.GamesPlayers.Find(2L);
                GamePlayer gp3 = context.GamesPlayers.Find(3L);
                GamePlayer gp4 = context.GamesPlayers.Find(4L);
                GamePlayer gp5 = context.GamesPlayers.Find(5L);
                GamePlayer gp6 = context.GamesPlayers.Find(6L);
                GamePlayer gp7 = context.GamesPlayers.Find(7L);
                GamePlayer gp8 = context.GamesPlayers.Find(8L);

                var salvos = new Salvo[]
                {
                    //jbauer gp1
                    new Salvo{ Turn = 1, GamePlayer = gp1, Locations = new SalvoLocation[]{ 
                        //locations
                        new SalvoLocation { Location = "B5"},
                        new SalvoLocation { Location = "C5"},
                        new SalvoLocation { Location = "F1"},
                    } },
                    new Salvo{ Turn = 2, GamePlayer = gp1, Locations = new SalvoLocation[]{ 
                        //locations
                        new SalvoLocation { Location = "F2"},
                        new SalvoLocation { Location = "F5"},
                    } },
                    //cobrian gp2
                    new Salvo{ Turn = 1, GamePlayer = gp2, Locations = new SalvoLocation[]{ 
                        //locations
                        new SalvoLocation { Location = "B4"},
                        new SalvoLocation { Location = "B5"},
                        new SalvoLocation { Location = "B6"},
                    } },
                    new Salvo{ Turn = 2, GamePlayer = gp2, Locations = new SalvoLocation[]{ 
                        //locations
                        new SalvoLocation { Location = "E1"},
                        new SalvoLocation { Location = "H3"},
                        new SalvoLocation { Location = "A2"},
                    } },
                    //jbauer gp3
                    new Salvo{Turn = 1, GamePlayer = gp3, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "A2" },
                            new SalvoLocation { Location = "A4" },
                            new SalvoLocation { Location = "G6" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp3, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "A3" },
                            new SalvoLocation { Location = "H6" }
                        }
                    },
                    //obrian gp4
                     new Salvo{Turn = 1, GamePlayer = gp4, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "B5" },
                            new SalvoLocation { Location = "D5" },
                            new SalvoLocation { Location = "C7" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp4, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "C5" },
                            new SalvoLocation { Location = "C6" }
                        }
                    },
                    //obiran gp5
                    new Salvo{Turn = 1, GamePlayer = gp5, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "G6" },
                            new SalvoLocation { Location = "H6" },
                            new SalvoLocation { Location = "A4" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp5, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "A2" },
                            new SalvoLocation { Location = "A3" },
                            new SalvoLocation { Location = "D8" }
                        }
                    },
                    //talmeida
                    new Salvo{Turn = 1, GamePlayer = gp6, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "H1" },
                            new SalvoLocation { Location = "H2" },
                            new SalvoLocation { Location = "H3" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp6, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "E1" },
                            new SalvoLocation { Location = "F2" },
                            new SalvoLocation { Location = "G3" }
                        }
                    },
                    //obrian gp7
                    new Salvo{Turn = 1, GamePlayer = gp7, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "A3" },
                            new SalvoLocation { Location = "A4" },
                            new SalvoLocation { Location = "F7" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp7, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "A2" },
                            new SalvoLocation { Location = "G6" },
                            new SalvoLocation { Location = "H6" }
                        }
                    },
                    //jbauer gp8
                    new Salvo{Turn = 1, GamePlayer = gp8, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "B5" },
                            new SalvoLocation { Location = "C6" },
                            new SalvoLocation { Location = "H1" }
                        }
                    },
                    new Salvo{Turn = 2, GamePlayer = gp8, Locations = new SalvoLocation[] {
                            new SalvoLocation { Location = "C5" },
                            new SalvoLocation { Location = "C7" },
                            new SalvoLocation { Location = "D5" }
                        }
                    },

                };

                foreach (Salvo salvo in salvos)
                {
                    context.Salvos.Add(salvo);
                }

                context.SaveChanges();
            }


            if (!context.Scores.Any())
            {
                Game game1 = context.Games.Find(1L);
                Game game2 = context.Games.Find(2L);
                Game game3 = context.Games.Find(3L);
                Game game4 = context.Games.Find(4L);

                Player jbauer = context.Players.Find(1L);
                Player obrian = context.Players.Find(2L);
                Player kbauer = context.Players.Find(3L);
                Player almeida = context.Players.Find(4L);

                var scores = new Score[]
                {
                    new Score{Point=1, FinishDate = DateTime.Now, Game = game1, Player = jbauer},
                    new Score{Point=0, FinishDate = DateTime.Now, Game = game1, Player = obrian},
                    new Score{Point=0.5, FinishDate = DateTime.Now, Game = game2, Player = jbauer},
                    new Score{Point=0.5, FinishDate = DateTime.Now, Game = game2, Player = obrian},
                    new Score{Point=1, FinishDate = DateTime.Now, Game = game3, Player = obrian},
                    new Score{Point=0, FinishDate = DateTime.Now, Game = game3, Player = almeida},

                     new Score{Point=0.5, FinishDate = DateTime.Now, Game = game4, Player = obrian},
                    new Score{Point=0.5, FinishDate = DateTime.Now, Game = game4, Player = jbauer},
                };

                foreach (Score score in scores)
                {
                    context.Scores.Add(score);
                }

                context.SaveChanges();
            }
        }
    }
}
