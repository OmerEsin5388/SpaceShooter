using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject PlayerBulletGO;
    public GameObject BulletPosition01;
    public GameObject BulletPosition02;
    public GameObject ExplosionGO;

    public Text LivesUIText;

    const int MaxLives = 3;
    int lives;

    public void Init()
    {
        lives = MaxLives;

        LivesUIText.text = lives.ToString ();

        gameObject.SetActive(true);
    }
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("space"))
        {
            GameObject bullet01 = (GameObject)Instantiate (PlayerBulletGO);
            bullet01.transform.position = BulletPosition01.transform.position;

            GameObject bullet02 = (GameObject)Instantiate (PlayerBulletGO);
            bullet02.transform.position = BulletPosition02.transform.position;
        }

       float x = Input.GetAxisRaw ("Horizontal"); // EN: the value will be -1, 0, or 1 (for left, no input, and right). TR: değer -1, 0 veya 1 olacaktır (sol, giriş yok ve sağ için).

       float y = Input.GetAxisRaw ("Vertical"); // EN: the value will be -1, 0, or 1 (for down, no input, and up). TR: değer -1, 0 veya 1 olacaktır (aşağı, giriş yok ve yukarı için).

       // EN: now based on the input we compute a direction vector, and we normalize it to get a unit vector. TR: şimdi girdiye dayanarak bir yön vektörü hesaplıyoruz ve bunu bir birim vektör elde etmek için normalleştiriyoruz.

       Vector2 direction = new Vector2 (x, y).normalized;
       // EN: now we call the function that computes and sets the player's position. TR: şimdi oyuncunun konumunu hesaplayan ve ayarlayan fonksiyonu çağırıyoruz.
       Move (direction);
    }
    void Move(Vector2 direction)
    {
        // EN: Find the screen limits to the player's movement (left, right, top, and bottom edges of the screen) TR: Oyuncunun hareketine ilişkin ekran sınırlarını bulun (ekranın sol, sağ, üst ve alt kenarları)
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0)); // EN: this is the bottom-left point (corner) of the screen. TR: burası ekranın sol alt noktasıdır (köşesi).
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1)); // EN: this is the top-right point (corner) of the screen. TR: burası ekranın sağ üst noktasıdır (köşesi).

        max.x = max.x - 0.225f; // EN: subtract the player sprite half width. TR: oynatıcı spriteının yarım genişliğini çıkarın.
        min.x = min.x + 0.225f; // EN: add the player sprite half width. TR: oynatıcı spriteının yarım genişliğini ekleyin.

        max.y = max.y - 0.285f; // EN: subtract the player sprite half height. TR: oynatıcı spriteının yarım uzunluğunu çıkarın.
        min.y = min.y + 0.285f; // EN: add the player sprite half height. TR: oynatıcı spriteının yarım uzunluğunu ekleyin.

        // EN: Get the player's current position. TR: Oyuncunun mevcut konumunu alın.

        Vector2 pos = transform.position;

        // EN: Calculate the new position. TR: Yeni konumu hesaplayın.

        pos += direction * speed * Time.deltaTime;

        // EN: Make sure the new position is not outside the screen. TR: Yeni konumun ekranın dışında olmadığından emin olun.

        pos.x = Mathf.Clamp (pos.x, min.x, max.x);
        pos.y = Mathf.Clamp (pos.y, min.y, max.y);

        // EN: Update the player's position. TR: Oyuncunun konumunu güncelleyin.

        transform.position = pos;
    }
    void OnTriggerEnter2D(Collider2D col) 
    {
        if((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
        {
            PlayExplosion();

            lives--;
            LivesUIText.text = lives.ToString();
            if(lives == 0)
            {
              GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
              gameObject.SetActive(false);
            }
        }

    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        explosion.transform.position = transform.position;
    }

}
