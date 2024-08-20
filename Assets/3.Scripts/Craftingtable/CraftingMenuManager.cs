using UnityEngine;
using UnityEngine.UI;

public class CraftingMenuManager : MonoBehaviour
{
    [System.Serializable]
    public struct Genre
    {
        public string name; 
        public GameObject genreObject;
        public Button genreButton;
    }
    [SerializeField] private Genre[] genres;
    [SerializeField] private Color defaultButtonColor;
    [SerializeField] private Color selectedButtonColor;

    void Start()
    {
        for (int i = 0; i < genres.Length; i++)
        {
            int index = i;
            genres[i].genreButton.onClick.AddListener(() => OnGenreButtonClick(index));
        }
        OnGenreButtonClick(0);
    }

    private void OnGenreButtonClick(int index)
    {
        for (int i = 0; i < genres.Length; i++)
        {
            genres[i].genreObject.SetActive(false);
            genres[i].genreButton.transform.parent.GetComponent<Image>().color = defaultButtonColor;
        }
        genres[index].genreObject.SetActive(true);
        genres[index].genreButton.transform.parent.GetComponent<Image>().color = selectedButtonColor;
    }
}