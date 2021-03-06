﻿using System;
using System.Collections.Generic;

namespace GrafFeladat_CSharp
{
    /// <summary>
    /// Irányítatlan, egyszeres gráf.
    /// </summary>
    class Graf
    {
        int csucsokSzama;
        /// <summary>
        /// A gráf élei.
        /// Ha a lista tartalmaz egy(A, B) élt, akkor tartalmaznia kell
        /// a(B, A) vissza irányú élt is.
        /// </summary>
        readonly List<El> elek = new List<El>();
        /// <summary>
        /// A gráf csúcsai.
        /// A gráf létrehozása után új csúcsot nem lehet felvenni.
        /// </summary>
        readonly List<Csucs> csucsok = new List<Csucs>();

        /// <summary>
        /// Létehoz egy úgy, N pontú gráfot, élek nélkül.
        /// </summary>
        /// <param name="csucsok">A gráf csúcsainak száma</param>
        public Graf(int csucsok)
        {
            this.csucsokSzama = csucsok;

            // Minden csúcsnak hozzunk létre egy új objektumot
            for (int i = 0; i < csucsok; i++)
            {
                this.csucsok.Add(new Csucs(i));
            }
        }

        /// <summary>
        /// Hozzáad egy új élt a gráfhoz.
        /// Mindkét csúcsnak érvényesnek kell lennie:
        /// 0 &lt;= cs &lt; csúcsok száma.
        /// </summary>
        /// <param name="cs1">Az él egyik pontja</param>
        /// <param name="cs2">Az él másik pontja</param>
        public void Hozzaad(int cs1, int cs2)
        {
            if (cs1 < 0 || cs1 >= csucsokSzama ||
                cs2 < 0 || cs2 >= csucsokSzama)
            {
                throw new ArgumentOutOfRangeException("Hibas csucs index");
            }

            // Ha már szerepel az él, akkor nem kell felvenni
            foreach (var el in elek)
            {
                if (el.Csucs1 == cs1 && el.Csucs2 == cs2)
                {
                    return;
                }
            }

            elek.Add(new El(cs1, cs2));
            elek.Add(new El(cs2, cs1));
        }

        public void Torles(int cs1, int cs2)
        {
            if (cs1 < 0 || cs1 >= csucsokSzama ||
                cs2 < 0 || cs2 >= csucsokSzama)
            {
                throw new ArgumentOutOfRangeException("Hibas csucs index");
            }

            //Csak akkor távolítja el, ha megtalálja az éllistában, tehát, ha létezik

            elek.Remove(elek.Find((x) => x.Csucs1 == cs1 && x.Csucs2 == cs2));
            elek.Remove(elek.Find((x) => x.Csucs1 == cs2 && x.Csucs2 == cs1));
        }

        public void SzelessegiBejar(int kezdopont)
        {
            var bejart = new HashSet<int>();
            var kovetkezok = new Queue<int>();
            int k = 0;

            if (kezdopont >= csucsokSzama || kezdopont < 0)
            {
                throw new ArgumentOutOfRangeException("Hibas kezdőpont");
            }

            kovetkezok.Enqueue(kezdopont);
            bejart.Add(kezdopont);

            while (kovetkezok.Count != 0)
            {
                k = kovetkezok.Dequeue();
                Console.WriteLine(this.csucsok[k]);

                foreach (var item in elek)
                {
                    if (item.Csucs1 == k && !bejart.Contains(item.Csucs2))
                    {
                        kovetkezok.Enqueue(item.Csucs2);
                        bejart.Add(item.Csucs2);
                    }
                }
            }
        }

        public void MelysegiBejar(int kezdopont)
        {
            var bejart = new HashSet<int>();
            bejart.Add(kezdopont);
            MelysegiBejarRekurziv(kezdopont, bejart);
        }

        void MelysegiBejarRekurziv(int k, HashSet<int> bejart)
        {
            Console.WriteLine(this.csucsok[k]);

            foreach (var item in elek)
            {
                if (item.Csucs1 == k && !bejart.Contains(item.Csucs2))
                {
                    bejart.Add(item.Csucs2);
                    MelysegiBejarRekurziv(item.Csucs2, bejart);
                }
            }
        }

        public bool Osszefuggo()
        {
            var bejart = new HashSet<int>();

            var kovetkezok = new Queue<int>();
            kovetkezok.Enqueue(0);
            bejart.Add(0);

            while (kovetkezok.Count != 0)
            {
                var k = kovetkezok.Dequeue();

                foreach (var item in elek)
                {
                    if (item.Csucs1 == k && !bejart.Contains(item.Csucs2))
                    {
                        kovetkezok.Enqueue(item.Csucs2);
                        bejart.Add(item.Csucs2);
                    }
                }
            }

            return bejart.Count == csucsokSzama;
        }

        public Graf Feszitofa()
        {
            var fa = new Graf(this.csucsokSzama);

            var bejart = new HashSet<int>();
            var kovetkezok = new Queue<int>();

            kovetkezok.Enqueue(0);
            bejart.Add(0);

            while (kovetkezok.Count != 0)
            {
                var aktualisCsucs = kovetkezok.Dequeue();

                foreach (var item in elek)
                {
                    if (item.Csucs1 == aktualisCsucs)
                    {
                        if (!bejart.Contains(item.Csucs2))
                        {
                            bejart.Add(item.Csucs2);
                            kovetkezok.Enqueue(item.Csucs2);
                            fa.Hozzaad(item.Csucs1, item.Csucs2);
                        }
                    }
                }
            }

            return fa;
        }

        public Dictionary<int, int> MohoSzinezes()
        {
            var szinezes = new Dictionary<int, int>();

            var maxSzin = csucsokSzama;

            for (int aktualisCsucs = 0; aktualisCsucs < csucsokSzama - 1; aktualisCsucs++)
            {
                var valaszthatoSzinek = new HashSet<int>();
                for (int i = 0; i < maxSzin - 1; i++)
                {
                    valaszthatoSzinek.Add(i);
                }

                foreach (var item in elek)
                {
                    if (item.Csucs1 == aktualisCsucs)
                    {
                        if (szinezes.ContainsKey(item.Csucs2))
                        {
                            var szin = szinezes[item.Csucs2];
                            valaszthatoSzinek.Remove(szin);
                        }
                    }
                }

                var st = new int[valaszthatoSzinek.Count];
                valaszthatoSzinek.CopyTo(st);
                int min = 0;
                for (int i = 1; i < st.Length; i++)
                {
                    if (st[i - 1] < st[i])
                    {
                        min = st[i - 1];
                    }
                }
                var valsztottSzin = min;
                szinezes.Add(aktualisCsucs, valsztottSzin);
            }

            return szinezes;
        }

        public override string ToString()
        {
            string str = "Csucsok:\n";
            foreach (var cs in csucsok)
            {
                str += cs + "\n";
            }
            str += "Elek:\n";
            foreach (var el in elek)
            {
                str += el + "\n";
            }
            return str;
        }
    }
}