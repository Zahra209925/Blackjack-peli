using System;
using System.Collections.Generic;

namespace Blackjack
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
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

				// Pelin pääsilmukka
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

					// Pelaajan valinta: "Hit", "Stand", tai "Näytä kortit"
					string valinta;
					do
					{
						Console.WriteLine("\nHaluatko ottaa uuden kortin (h), jäädä (s), vai nähdä jäljellä olevat kortit (n)?");
						valinta = Console.ReadLine().ToLower();
						if (valinta != "h" && valinta != "s" && valinta != "n")
						{
							Console.WriteLine("Virheellinen valinta, yritä uudelleen.");
						}
					} while (valinta != "h" && valinta != "s" && valinta != "n");

					if (valinta == "h")
					{
						pelaajanKasi.Add(deck.DrawCard());
					}
					else if (valinta == "s")
					{
						pelaajanVuoro = false;
					}
					else if (valinta == "n")
					{
						Console.WriteLine("\nJäljellä olevat kortit pakassa:");
						deck.NaytaJaljellaOlevatKortit();
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
				Console.WriteLine($"Jakajan käden arvo: {LaskeKadenArvo(jakajanKasi)}");

				// Voittajan selvittäminen
				int pelaajanArvo = LaskeKadenArvo(pelaajanKasi);
				int jakajanArvo = LaskeKadenArvo(jakajanKasi);

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

				// Kysytään, haluaako pelaaja pelata uudelleen
				Console.WriteLine("\nHaluatko pelata uudelleen? (k/e)");
				string uudelleen = Console.ReadLine().ToLower();
				if (uudelleen != "k")
				{
					break;
				}
				Console.Clear();
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
		public string Maa { get; set; } // Maat: Zahra, Sara, Anna, Zeinab
		public string ArvoNimi { get; set; } // Kortin arvo: 2-10, J, Q, K, A
		public int Arvo { get; set; }   // Kortin numeerinen arvo

		public Card(string maa, string arvoNimi, int arvo)
		{
			Maa = maa;
			ArvoNimi = arvoNimi;
			Arvo = arvo;
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
			string[] maat = { "Zahra", "Sara", "Anna", "Zeinab" };
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

		// Korttipakan sekoittaminen
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
			if (kortit.Count == 0)
			{
				throw new InvalidOperationException("Korttipakka on tyhjä!");
			}
			Card card = kortit[0];
			kortit.RemoveAt(0);
			return card;
		}

		// Näytä jäljellä olevat kortit
		public void NaytaJaljellaOlevatKortit()
		{
			if (kortit.Count == 0)
			{
				Console.WriteLine("Korttipakka on tyhjä!");
			}
			else
			{
				foreach (var kortti in kortit)
				{
					Console.WriteLine(kortti);
				}
			}
		}
	}
}


