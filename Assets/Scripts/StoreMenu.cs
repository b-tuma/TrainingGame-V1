using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public Button productPrefab;
    public RectTransform startPoint;
    public float productSeparation = 25f;
    private GameManager mGameManager;
    private StoreDatabase mStoreDatabase;
    private int storeDatabaseLength;
    private Button[] mProducts;
    public RectTransform productHolder;
    private float productHolderMovement;
    public Scrollbar scrollbar;
    private float productHolderOriginal;
    public Text myMoney;

    void Awake()
    {
        mGameManager = GameManager.gm;
        mStoreDatabase = mGameManager.GetComponent<StoreDatabase>();
        storeDatabaseLength = mStoreDatabase.products.Count;
        if (storeDatabaseLength == 0) return;
        mProducts = new Button[storeDatabaseLength];
        float topY = 0;
        float bottomY = 0;
        for (int i = 0; i < storeDatabaseLength; i++)
        {
            BaseProduct product = mStoreDatabase.products[i];
            mProducts[i] = Instantiate(productPrefab);
            mProducts[i].transform.FindChild("Name").GetComponent<Text>().text = product.name;
            mProducts[i].transform.FindChild("Desc").GetComponent<Text>().text = product.description;
            mProducts[i].transform.FindChild("Price").GetComponent<Text>().text = product.price.ToString();
            mProducts[i].GetComponent<Image>().sprite = product.image;
            RectTransform productRect = mProducts[i].GetComponent<RectTransform>();
            productRect.SetParent(productHolder.parent);
            float instantiatePos = startPoint.anchoredPosition.y - ((productRect.sizeDelta.y + productSeparation)*i);
            productRect.anchoredPosition = new Vector2(startPoint.anchoredPosition.x, instantiatePos);
            productRect.localScale = Vector3.one;
            if (i == 0)
            {
                topY = productRect.anchoredPosition.y + (productRect.sizeDelta.y/2);
            }
            if (i == storeDatabaseLength - 1)
            {
                bottomY = productRect.anchoredPosition.y - (productRect.sizeDelta.y/2);
            }
            mProducts[i].onClick.AddListener(() => { BuyItem(product); });

        }
        productHolder.sizeDelta = new Vector2(productHolder.sizeDelta.x, topY - bottomY + 40f);
        productHolderMovement = productHolder.sizeDelta.y -
                                productHolder.transform.parent.GetComponent<RectTransform>().sizeDelta.y + productSeparation;
        foreach (var productB in mProducts)
        {
            productB.transform.SetParent(productHolder.transform);
        }
        productHolderOriginal = productHolder.anchoredPosition.y;
    }

    public void BuyItem(BaseProduct product)
    {
        if (product.price <= mGameManager.coinNumber)
        {
            mGameManager.coinNumber -= product.price;
        }
    }

    void FixedUpdate()
    {
        float holderPosition = productHolderMovement*scrollbar.value;
        productHolder.anchoredPosition = new Vector2(productHolder.anchoredPosition.x, productHolderOriginal + holderPosition);
        myMoney.text = mGameManager.coinNumber.ToString();
    }

    public void ReturnButton()
    {
        mGameManager.LoadLevel(mGameManager.lastScene);
    }
}
