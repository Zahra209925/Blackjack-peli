using System;
using System.Collections.Generic;

namespace Blackjack
{
	class program
	{
		static void Main(string[] args)
		{
			// Tervetuloa-painatus
			Console.WriteLine("Tervetuloa Blackjack-peliin!");
			Console.WriteLine("Tavoitteesi on saada korttien summa mahdollisimman lähelle 21:tä menemättä yli.");
			Console.WriteLine("Aloitetaan peli\n");

			// Korttipakka ja pelaajan sekä jakajan kädet
			Deck deck = new Deck();
			deck.Sekoita();

			List<Card> pelaajanKasi = new List<Card>();
			List<Card> jakajanKasi = new List<Card>();

			// Pelaajalle ja jakajalle jaetaan aluksi kaksi korttia
			pelaajanKasi.Add(deck.DrawCard());
			pelaajanKasi.Add(deck.DrawCard());
			jakajanKasi.Add(deck.DrawCard());
			jakajanKasi.Add(deck.DrawCard());

			// pelin pääsilmukka
			bool pelaajanVuoro = true;
			while (pelaajanVuoro)
			{
				Console.WriteLine("\nKätesi:");
				TulostaKasi(pelaajanKasi);
				Console.WriteLine($"Kätesi arvo: {LaskeKadenArvo(pelaajanKasi)}");

				Console.WriteLine("\nJakajan näkyvä kortti:");
				Console.WriteLine(jakajanKasi[0]);

				// Tarkistetaan, onko pelaaja voittanut tai hävinnyt
				if (LaskeKadenArvo(pelaajanKasi) > 21)
				{
					Console.WriteLine("Ylitit 21! Hävisit pelin.");
					return;
				}
				// Pelaajan valinta: "Hit" tai "Stand"
				Console.WriteLine("\nHaluatko ottaa uuden kortin (h) vai jäädä (s)?");
				string valinta = Console.ReadLine();
				if (valinta.ToLower() == "h")
				{
					pelaajanKasi.Add(deck.DrawCard());
				}
				else if (valinta.ToLower() == "s")
				{
					pelaajanVuoro = false;
				}
				else
				{
					Console.WriteLine("Virheellinen valinta, yritä uudelleen.");
				}
			}

			// Jakajan vuoro
			Console.WriteLine("\nJakajan vuoro...");
			while (LaskeKadenArvo(jakajanKasi) < 17)
			{
				jakajanKasi.Add(deck.DrawCard());
			}

			// Tulostetaan lopulliset kädet
			Console.WriteLine("\nLopulliset kädet:");
			Console.WriteLine("Pelaajan käsi:");
			TulostaKasi(pelaajanKasi);
			Console.WriteLine($"Pelaajan käden arvo: {LaskeKadenArvo(pelaajanKasi)}");

			Console.WriteLine("\nJakajan käsi:");
			TulostaKasi(jakajanKasi);
			Console.WriteLine($"Jakajan käden arvo: {LaskeKadenArvo(pelaajanKasi)}");

			// Voittajan selvittäminen
			int pelaajanArvo = LaskeKadenArvo(pelaajanKasi);
			int jakajanArvo = LaskeKadenArvo(pelaajanKasi);

			if (pelaajanArvo > 21)
			{
				Console.WriteLine("Hävisit! Ylitit 21.");
			}
			else if (jakajanArvo > 21 || pelaajanArvo > jakajanArvo)
			{
				Console.WriteLine("Voitit! Onneksi olkoon!");
			}
			else if (pelaajanArvo == jakajanArvo)
			{
				Console.WriteLine("Tasapeli!");
			}
			else
			{
				Console.WriteLine("Hävisit! Jakaja voitti.");
			}
		}

		// Funktio korttien tulostamiseen
		static void TulostaKasi(List<Card> kasi)
		{
			foreach (var kortti in kasi)
			{
				Console.WriteLine(kortti);
			}
		}

		// Funktio käden arvon laskemiseen
		static int LaskeKadenArvo(List<Card> kasi)
		{
			int arvo = 0;
			int assienMaara = 0;

			foreach (var kortti in kasi)
			{
				arvo += kortti.Arvo;
				if (kortti.ArvoNimi == "A")
				{
					assienMaara++;
				}
			}

			// Ässän arvo voi olla 1 tai 11
			while (arvo > 21 && assienMaara > 0)
			{
				arvo -= 10;
				assienMaara--;
			}
			return arvo;

		}
	}

	// Korttiluokka
	class Card
	{
		public string Maa { get; set; } // Maat: Hertta, Ruutu, Risti, Pata
		public string ArvoNimi { get; set; } // Kortin arvo: 2-10, J, Q, K, A
		public int Arvo { get; set; }   // Kortin numeerinen arvo

		public Card(string maa, string ArvoNimi, int arvo)
		{
			Maa = maa;
			ArvoNimi = ArvoNimi;
			arvo = arvo;
		}

		public override string ToString()
		{
			return $"{ArvoNimi} {Maa}";
		}
	}

	// Korttipakkaluokka
	class Deck
	{
		private List<Card> kortit;

		public Deck()
		{
			kortit = new List<Card>();
			string[] maat = { "Zahra", "Sara", "Rabi", "Milla" };
			string[] arvot = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
			int[] numeerisetArvot = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

			// Luodaan korttipakka
			for (int i = 0; i < maat.Length; i++)
			{
				for (int j = 0; j < arvot.Length; j++)
				{
					kortit.Add(new Card(maat[i], arvot[j], numeerisetArvot[j]));
				}
			}
		}

		// Korttipakan sekoittaminens
		public void Sekoita()
		{
			Random rng = new Random();
			int n = kortit.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Card value = kortit[k];
				kortit[k] = kortit[n];
				kortit[n] = value;
			}
		}

		// Kortin nostaminen pakasta
		public Card DrawCard()
		{
			Card card = kortit[0];
			kortit.RemoveAt(0);
			return card;
		}
	}
}

