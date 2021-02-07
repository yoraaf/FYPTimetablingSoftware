using System;
using System.Collections.Generic;
using System.Text;
/*
public class TestShakespeare {

    string targetString = "To be, or not to be, that is the question.";
    string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
    int populationSize = 200;
    float mutationRate = 0.01f;
    int elitism = 5;
    bool enabled = true;

    int numCharsPerText = 15000;

    private GeneticAlgorithm<char> ga;
    private System.Random random;

    void Start() {
        //targetText.text = targetString;

        if (string.IsNullOrEmpty(targetString)) {
            Console.Error.WriteLine("Target string is null or empty");
            enabled = false;
        }

        random = new System.Random();
        ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, elitism, mutationRate);
    }

    
	 * make some kind of timer function or recursive function
	 * 
	void Update() {
	add pause button
		ga.NewGeneration();

		UpdateText(ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count, (j) => ga.Population[j].Genes);

		if (ga.BestFitness == 1) {
			this.enabled = false;
		}
	}
	
    private char GetRandomCharacter() {
        int i = random.Next(validCharacters.Length);
        return validCharacters[i];
    }

    private float FitnessFunction(int index) {
        float score = 0;
        DNA<char> dna = ga.Population[index];

        for (int i = 0; i < dna.Genes.Length; i++) {
            if (dna.Genes[i] == targetString[i]) {
                score += 1;
            }
        }

        score /= targetString.Length;

        score = (float)((Math.Pow(2, score) - 1) / (2 - 1));

        return score;
    }

	
	 * 
	 * make this all work with winForm
	 * 
	private int numCharsPerTextObj;
	private List<Text> textList = new List<Text>();

	void Awake() {
		numCharsPerTextObj = numCharsPerText / validCharacters.Length;
		if (numCharsPerTextObj > populationSize) numCharsPerTextObj = populationSize;

		int numTextObjects = Mathf.CeilToInt((float)populationSize / numCharsPerTextObj);

		for (int i = 0; i < numTextObjects; i++) {
			textList.Add(Instantiate(textPrefab, populationTextParent));
		}
	}

	private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize, Func<int, char[]> getGenes) {
		//add label text updates here
		bestText.text = CharArrayToString(bestGenes);
		bestFitnessText.text = bestFitness.ToString();
		numGenerationsText.text = generation.ToString();

		for (int i = 0; i < textList.Count; i++) {
			var sb = new StringBuilder();
			int endIndex = i == textList.Count - 1 ? populationSize : (i + 1) * numCharsPerTextObj;
			for (int j = i * numCharsPerTextObj; j < endIndex; j++) {
				foreach (var c in getGenes(j)) {
					sb.Append(c);
				}
				if (j < endIndex - 1) sb.AppendLine();
			}

			textList[i].text = sb.ToString();
		}

	}
	private string CharArrayToString(char[] charArray){
		var sb = new StringBuilder();
		foreach (var c in charArray)
		{
			sb.Append(c);
		}

		return sb.ToString();
	}
	
}
*/