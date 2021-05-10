using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryManager : MonoBehaviour
{
    public static LaundryManager instance;

    public int MinimumNumberOfGarments;
    public int MaximumNumberOfGarments;

    [System.Serializable]
    public struct FabricParameters {
        public int ratio;
        public FabricType fabric;
        public GarmentType[] garmentType;
    }

    public FabricParameters[] LaundryGenerationParameters;

    [HideInInspector]
    public int[] WeightedFabricIndices;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        List<int> weightedFabricIndices = new List<int>();
        for (int i = 0; i < LaundryGenerationParameters.Length; i++)
            for (int j = 0; j < LaundryGenerationParameters[i].ratio; j++)
                weightedFabricIndices.Add(i);
        WeightedFabricIndices = weightedFabricIndices.ToArray();
    }

    public static Basket GetRandomBasket() {
        Basket basket = new Basket();

        //Determine number of garments
        int numberOfGarments = UnityEngine.Random.Range(instance.MinimumNumberOfGarments, instance.MaximumNumberOfGarments + 1);

        List<Garment> garments = new List<Garment>();

        //Generate the garments
        for(int i = 0; i < numberOfGarments; ) {
            //Determine fabricType
            int fabricIndex = instance.WeightedFabricIndices[UnityEngine.Random.Range(0, instance.WeightedFabricIndices.Length)];
            FabricType fabricType = instance.LaundryGenerationParameters[fabricIndex].fabric;

            //Determine garmentType
            int garmentTypeIndex = UnityEngine.Random.Range(0, instance.LaundryGenerationParameters[fabricIndex].garmentType.Length);
            GarmentType garmentType = instance.LaundryGenerationParameters[fabricIndex].garmentType[garmentTypeIndex];

            //Instantiate garment
            Garment garment = null;
            switch (garmentType) {
                case GarmentType.Top:
                    garment = new GarmentTop(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Pants:
                    garment = new GarmentPants(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Sock:
                    garment = new GarmentSock(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Underwear:
                    garment = new GarmentUnderwear(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Dress:
                    garment = new GarmentDress(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Skirt:
                    garment = new GarmentSkirt(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                case GarmentType.Shirt:
                    garment = new GarmentShirt(new Fabric(fabricType), GarmentColor.RandomColor());
                    break;
                default:
                    Debug.LogError("Unknown garment type");
                    break;
            }

            i += garment.size;
            garments.Add(garment);
            if (garment is GarmentSock) {
                Garment otherSock = new GarmentSock((GarmentSock)garment);
                garments.Add(otherSock);
                i += otherSock.size;
            }

        }

        //Shuffle clothing to separate socks
        int n = garments.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Garment value = garments[k];
            garments[k] = garments[n];
            garments[n] = value;
        }

        foreach (Garment garment in garments)
            basket.AddGarment(garment);
        
        return basket;
    }

    private bool WoolShrunk;
    private bool WhiteDyed;
    private bool LowOnlyTorn;
    private bool HangOnlyTorn;
    private bool SynthMelted;
    private bool GarmentBurned;

    private void OnEnable() {
        LaundryObject.Grabbed += DisplayMistakeToast;
    }

    private void OnDisable() {
        LaundryObject.Grabbed -= DisplayMistakeToast;
    }

    private void DisplayMistakeToast(LaundryObject laundryObject) {
        if(laundryObject is LaundryGarment laundryGarment) {
            Garment garment = laundryGarment.garment;
            if (garment.Ruined) {
                if (!GarmentBurned && garment.Burned) {
                    //Garment burned
                    GarmentBurned = true;
                    ToastManager.instance.SayLine("Watch out with that iron! Keep it movin', and don't overdo it!", 1.0f);
                }
                if(!SynthMelted && garment.Melted) {
                    //Synth melted
                    SynthMelted = true;
                    ToastManager.instance.SayLine("Did ya melt some synthetic fabric? Smells like burnt hair in there!", 1.0f);
                }
                if(!LowOnlyTorn && garment.Torn && garment.fabric.dryingRestrictions == DryingRestrictions.LowOnly) {
                    //LowOnlyTorn
                    LowOnlyTorn = true;
                    ToastManager.instance.SayLine("Wow, those are messed up. Maybe use a lower TUMBLE setting next time!", 1.0f);
                }
                if(!HangOnlyTorn && garment.Torn && garment.fabric.dryingRestrictions == DryingRestrictions.HangDryOnly) {
                    //HangOnlyTorn
                    HangOnlyTorn = true;
                    ToastManager.instance.SayLine("Hey, that's delicate! Don't put it in the dryer, or it'll get shredded!", 1.0f);
                }
                if(!WhiteDyed && garment.Dyed) {
                    //WhiteDyed
                    WhiteDyed = true;
                    ToastManager.instance.SayLine("Say... that was white before you washed it, wasn't it?", 1.0f);
                    ToastManager.instance.SayLine("When doin' a HOT WASH, don't forget to separate the WHITE clothes the COLORED ones!", 1.0f);
                }
                if(!WoolShrunk && garment.Shrunk) {
                    //WoolShrunk
                    WoolShrunk = true;
                    ToastManager.instance.SayLine("Whoops, looks like the COLD water shrunk the WOOL... Be careful!", 1.0f);
                }
            }
        }
    }
}