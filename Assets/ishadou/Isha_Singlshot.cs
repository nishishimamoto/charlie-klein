using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Isha_Singlshot : MonoBehaviour
{
    [SerializeField] CursorSelect cursorSelectCS;
    [SerializeField] Turn TurnCS;
    [SerializeField] Timer TimerCS;
    [SerializeField] Combo ComboCS;
    [SerializeField] Explosion ExplosionCS;
    [SerializeField] Pause PauseCS;

    const int mainPanel = 30;    //メインパネルの数
    const int sidePanel = 42;    //サイドパネルの数
    const int width = 7;   //横の数
    const int height = 6;    //縦の数

    int[] mainNumber = new int[mainPanel];     //3*3のナンバー
    int[] sideNumber = new int[sidePanel];     //4*4のナンバー
    int tmpNumber;          //数字入れ替え時の一時保存
    int[] bonusLevel = new int[sidePanel]; //サイドパネルのボーナス確認 (0=なし,1=あり)
    bool[] bonusFlg = new bool[sidePanel]; //このターン既にボーナスパネルになったかどうか
    int tmpBonus;         //ボーナス入れ替え時の一時保存
    int judgNum = 0;  //和を計算する配列
    public static int score;      //スコア

    int chooseMain = -1; //現在選んでいるメインナンバー

    bool isHorizontal;     //十字キーの左右入力がニュートラルにもどったか
    bool isVertical;    //十字キーの上下入力がニュートラルにもどったか

    //[SerializeField] Image[] sideImage; //サイドスフィアをいれる
    GameObject[] sideSphere = new GameObject[sidePanel]; //サイドスフィアをいれる
    Color[] sideSphereColor = new Color[sidePanel];  //マテリアル色を変えるための仮入れ
    Color tmpSideColor;
    GameObject tmpObj;
    Renderer[] sideSphereRenderer = new Renderer[sidePanel];    //実際にオブジェクトの色を変更する
    PanelAnim[] panelAnim = new PanelAnim[sidePanel];
    PanelAnim tmpAnim;

    GameObject[] mainSphere = new GameObject[mainPanel];
    Color[] mainSphereColor = new Color[mainPanel];  //マテリアル色を変えるための仮入れ
    Color tmpMainColor;
    Renderer[] mainSphereRenderer = new Renderer[mainPanel];    //実際にオブジェクトの色を変更する

    int[] mainColorNumber = { 4, 32, 128, 512 };    //メイン色の配列(赤、青、緑、黄)
    int[] sideColorNumber = { 1, 8, 32, 128 };     //サイド色の配列(赤、青、緑、黄)
    int[] rainbowNumber = { 0, 1, width, width + 1 };           //虹衛星を出すときに使う
    bool rainbow;
    int[] rainbowRand = new int[mainPanel]; //虹衛星をランダムに配置するための変数
    int rainbowTarget = 0;
    //[SerializeField] GameObject selectMainImage; //現在選択しているメインパネルを表示

    [SerializeField] GameObject Score;  //スコアのテキストオブジェクト
    Text scoreText;
    int[] targetNum = new int[4];
    [SerializeField] Text[] targetText = new Text[1];
    [SerializeField] GameObject gameClear;

    float alpha_Time = 0f;   //点滅させる時間
    float alpha_Sin;    //消すときに点滅させる
    bool alpha_Flg;
    int check = 0; //中身を順にみる変数

    bool[] flgCheck = new bool[mainPanel + 1];  //ポイントになった箇所を記憶,5はnull
    int mainColorNum = 0;               //全パネルが同じ色になったら色を変える
    int checkColorNum;

    bool[] panelMove = new bool[2]; //右か左にパネル移動させるフラグ
                                    //float changeTime = 0.1f;

    public Material[] _material;           // 割り当てるマテリアル.
    public Texture NormalmapTexture;

    [SerializeField] GameObject main;
    [SerializeField] GameObject side;
    [SerializeField] bool[] isPlanet = new bool[mainPanel];

    public static string oldSceneName;  //リザルトから戻る用

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 4; i++)
        {
            mainNumber[i] = mainColorNumber[i];
            targetNum[i] = 3;
        }

        mainSphere[0] = Instantiate(main, new Vector3(7.7f, 1.5f + (-1.5f * 1), 0), Quaternion.identity);
        

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                //プレハブを元に、インスタンスを生成
                if (chooseMain < 0) chooseMain = i;
            }
        }

        for (int i = 0; i < sidePanel; i++) //衛星の生成
        {
            if (i <= 6)  //最上行のとき
            {
                if (i == 0 && isPlanet[0]) //隅の惑星がなければ隅の衛星も作らない
                {
                }
                else if (i == 6 && isPlanet[5]) //隅の惑星がなければ隅の衛星も作らない
                {
                }
                else if (i != 0 && i != 6 && isPlanet[i - (i / width) - 1] && isPlanet[i - (i / width)])//惑星1列目を見て、両隣がなければ作らない
                {
                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                }
            }
            else if (i > 0 && i % 7 == 0) //最左列のとき
            {
                if (i == 35 && isPlanet[24]) //隅の惑星がなければ隅の衛星も作らない
                {

                }
                else if (i != 35 && isPlanet[i - (i / width) - 6] && isPlanet[i - (i / width)]) //惑星最左列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                }
            }
            else if (i > 6 && i % 7 == 6)   //最右列のとき
            {
                if (i == 41 && isPlanet[29])//隅の惑星がなければ隅の衛星も作らない
                {

                }
                else if (i != 41 && isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - 1]) //惑星最右列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                }
            }
            else if (i > 35 && i < 41)  //最下行の時
            {
                if (isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - (width - 1)])//惑星最下列を見て、両隣がなければ作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                }
            }
            else
            {
                if (isPlanet[i - (i / width) - width] && isPlanet[i - (i / width) - (width - 1)] && isPlanet[i - (i / width) - 1] && isPlanet[i - (i / width)])//四方に惑星がない場合は衛星を作らない
                {

                }
                else
                {
                    //プレハブを元に、インスタンスを生成、
                    sideSphere[i] = Instantiate(side, new Vector3(-5 + (2 * (i % width - 1)), 5 - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = sideColorNumber[Random.Range(0, 4)];
                }
            }
       }

        scoreText = Score.GetComponent<Text>();

        ColorChange();   //パネルの色変更
        TurnCS.nowTurn = 1; //ターン数の指定
        TimerCS.maxTime = 60.0f;
        TimerCS.timeCount = TimerCS.maxTime;
        score = 0;  //スコアの初期化
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!PauseCS.isPause)
        {
            if (!alpha_Flg)
            {
                if (!panelMove[0] && !panelMove[1]) PanelOperation();   //パネルの操作
                else if (panelMove[0] || panelMove[1]) PanelMove();        //パネルのアニメーション
                PointCheck();             //盤面が揃ったか見る 揃ったらすぐ変わる
                //PointBlinking();            //4つ揃ったときの点滅
                //ColorChange();              //パネルの色変更
                TimerCS.TimerCount();       //制限時間のカウントと表示
                TurnEnd();                  //ターン終了時の処理

                //SelectImageMove();  //現在選んでいるパネルの可視化 ここで呼ぶ
                cursorSelectCS.SelectImageMove(chooseMain);

            }
            else if (alpha_Flg) alpha();
        }

        targetText[0].text = "x " + targetNum[0];

    }

    //***
    void alpha()
    {
        if ((flgCheck[check] && check <= (mainPanel - 1)) && !isPlanet[check])   //0~9で条件を満たしたら
        {

            if (alpha_Time < 0.6f)    //条件を満たしたパネルの点滅
            {
                alpha_Time += Time.deltaTime * 2;    //制限時間のカウントダウン
                alpha_Sin = (alpha_Time * 6) + 5;
                //alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;

                //mainSphere[check].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);　草
                sideSphere[(check / (width - 1)) + check].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
                sideSphere[(check / (width - 1)) + check + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", alpha_Sin);
            }
            else if (alpha_Time >= 0.6f)
            {
                alpha_Time = 0;

                //mainNumber[i] = mainColorNumber[0];
                sideNumber[(check / (width - 1)) + check] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + 1] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + width + 1] = sideColorNumber[Random.Range(0, 4)];
                sideNumber[(check / (width - 1)) + check + width] = sideColorNumber[Random.Range(0, 4)];

                //mainColorNum += mainNumber[check];  //[0]^[3]合計を得る草

                rainbowRand[rainbowTarget] = check; //条件を満たした惑星の位置を把握しておく
                rainbowTarget += 1;


                ScoreAdd();


                ////ランダムな数値にいれかえ
                //MainGenerate();

                ExplosionCS.particle[check].Play(); //条件を満たした惑星が爆発
                Invoke("ExplosionStop", 1.0f);    //時間差で爆発を止める
                ClearCheck(); //クリア条件を満たしたかチェック
                ColorChange();   //パネルの色変更

                check += 1;
            }
        }
        else if (check > (mainPanel - 1))    //最後に盤面を変える
        {
            if (alpha_Sin <= 8)    //半透明から透明へ
            {
                alpha_Sin += Time.deltaTime * 2;

                for (int i = 0; i < mainPanel; i++)
                {
                    if (flgCheck[i])
                    {
                        //mainSphere[i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);　草
                        sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                        sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 8);
                    }
                }
            }
            else
            {
                Bonus();    //ボーナスパネル設定
                for (int f = 0; f < mainPanel; f++) flgCheck[f] = false; //念のため別のforでfalseにする
                mainColorNum = 0;
                ColorChange();   //パネルの色変更
                check = 0;
                rainbow = false;
                alpha_Flg = false;
                rainbowTarget = 0;
            }
        }
        else check += 1;
    }
    //***
    void PanelOperation()
    {
        //パネル反時計回り
        if (Input.GetButtonDown("LB"))
        {
            panelMove[0] = true;
            if (!TimerCS.countStart) TimerCS.countStart = true;
            ComboCS.comboCount = 0;
        }
        //パネル時計回り
        else if (Input.GetButtonDown("RB"))
        {
            panelMove[1] = true;
            if (!TimerCS.countStart) TimerCS.countStart = true;
            ComboCS.comboCount = 0;
        }

        //十字キーのパネル選択
        if (0 > Input.GetAxis("ClossVertical") && !isVertical)    //↓入力時
        {
            do
            {
                if (chooseMain >= 24) chooseMain -= 24;
                else chooseMain += 6;
            } while (isPlanet[chooseMain]);
            isVertical = true;
        }
        if (0 < Input.GetAxis("ClossVertical") && !isVertical)  //↑入力時
        {
            do
            {
                if (chooseMain <= 5) chooseMain += 24;
                else chooseMain -= 6;
            } while (isPlanet[chooseMain]);
            isVertical = true;
        }
        if (0 > Input.GetAxis("ClossHorizontal") && !isHorizontal)  //←入力時
        {
            do
            {
                if (chooseMain % (width - 1) == 0) chooseMain += 5;
                else chooseMain -= 1;
            } while (isPlanet[chooseMain]);
            isHorizontal = true;
        }
        if (0 < Input.GetAxis("ClossHorizontal") && !isHorizontal)    //→入力時
        {
            do
            {
                if (chooseMain % (width - 1) == 5) chooseMain -= 5;
                else chooseMain += 1;
            } while (isPlanet[chooseMain]);
            isHorizontal = true;
        }

        if (0 == Input.GetAxis("ClossHorizontal")) isHorizontal = false;
        if (0 == Input.GetAxis("ClossVertical")) isVertical = false;
    }
    //***
    void PointCheck()
    {

        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                judgNum = mainNumber[0];

                if (sideNumber[(i / (width - 1)) + i] * 4 == sideNumber[(i / (width - 1)) + i + 1] * 4)
                    if (sideNumber[(i / (width - 1)) + i + 1] * 4 == sideNumber[(i / (width - 1)) + i + width + 1] * 4)
                        if (sideNumber[(i / (width - 1)) + i + width + 1] * 4 == sideNumber[(i / (width - 1)) + i + width] * 4)
                        {
                            flgCheck[i] = true;
                            alpha_Flg = true;
                            ComboCS.comboCount += 1;
                            if (mainNumber[0] == sideNumber[(i / (width - 1)) + i + width] * 4) //色を満たした
                            {
                                if (targetNum[0] > 0) targetNum[0] -= 1;

                            }
                        }

                judgNum = 0;

            }
        }
    }

    //***
    public void ColorChange()
    {
            switch (mainNumber[0])
            {
                case 4:
                    //mainSphereColor[i] = Color.red;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[0].GetComponent<Renderer>().material = _material[0];
                    break;
                case 32:
                    //mainSphereColor[i] = Color.blue;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[0].GetComponent<Renderer>().material = _material[1];
                    break;
                case 128:
                    //mainSphereColor[i] = Color.yellow;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[0].GetComponent<Renderer>().material = _material[2];
                    break;
                case 512:
                    //mainSphereColor[i] = Color.green;
                    //mainSphere[i].GetComponent<Renderer>().material.color = mainSphereColor[i];
                    mainSphere[0].GetComponent<Renderer>().material = _material[3];
                    break;
                default:
                    break;
            }

        for (int i = 0; i < sidePanel; i++)
        {
            switch (sideNumber[i])
            {
                case 1:
                    //sideSphereColor[i] = Color.red;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[0];
                    break;
                case 8:
                    //sideSphereColor[i] = Color.blue;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[1];
                    break;
                case 32:
                    //sideSphereColor[i] = Color.yellow;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[2];
                    break;
                case 128:
                    //sideSphereColor[i] = Color.green;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[3];
                    break;
                default:
                    break;
            }
        }
    }
    //***
    void Bonus()
    {
        for (int f = 0; f < sidePanel; f++)
        {
            //ボーナスフラグがあれば+1なければ-1(下限0)
            if (bonusLevel[f] > 0 && !bonusFlg[f]) bonusLevel[f] -= 1;
            if (bonusFlg[f]) bonusLevel[f] = 1;

            bonusFlg[f] = false;
        }
    }
    //***
    void PanelMove()    //アニメーション終了時に呼ぶ
    {
        if (panelMove[0])   //反時計周り
        {
            //パネルの回転アニメーション
            panelAnim[(chooseMain / (width - 1)) + chooseMain].animFlg[1] = true; //down
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1].animFlg[2] = true; //left
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1].animFlg[3] = true; //up
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width].animFlg[0] = true; //right

            //ナンバー入れ替え
            tmpNumber = sideNumber[(chooseMain / (width - 1)) + chooseMain];
            sideNumber[(chooseMain / (width - 1)) + chooseMain] = sideNumber[(chooseMain / (width - 1)) + chooseMain + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width] = tmpNumber;

            //ボーナス入れ替え
            tmpBonus = bonusLevel[(chooseMain / (width - 1)) + chooseMain];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width] = tmpBonus;

            //オブジェクト入れ替え
            tmpObj = sideSphere[(chooseMain / (width - 1)) + chooseMain];
            sideSphere[(chooseMain / (width - 1)) + chooseMain] = sideSphere[(chooseMain / (width - 1)) + chooseMain + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width] = tmpObj;

            //スクリプト入れ替え
            tmpAnim = panelAnim[(chooseMain / (width - 1)) + chooseMain];
            panelAnim[(chooseMain / (width - 1)) + chooseMain] = panelAnim[(chooseMain / (width - 1)) + chooseMain + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width] = tmpAnim;

            panelMove[0] = false;
        }
        else if (panelMove[1])  //時計周り
        {
            //パネルの回転アニメーション
            panelAnim[(chooseMain / (width - 1)) + chooseMain].animFlg[4] = true; //right2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1].animFlg[5] = true; //down2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1].animFlg[6] = true; //up2
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width].animFlg[7] = true; //left2

            //ナンバー入れ替え
            tmpNumber = sideNumber[(chooseMain / (width - 1)) + chooseMain];
            sideNumber[(chooseMain / (width - 1)) + chooseMain] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width] = sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideNumber[(chooseMain / (width - 1)) + chooseMain + 1];
            sideNumber[(chooseMain / (width - 1)) + chooseMain + 1] = tmpNumber;

            //ボーナス入れ替え
            tmpBonus = bonusLevel[(chooseMain / (width - 1)) + chooseMain];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + width + 1] = bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1];
            bonusLevel[(chooseMain / (width - 1)) + chooseMain + 1] = tmpBonus;

            //オブジェクト入れ替え
            tmpObj = sideSphere[(chooseMain / (width - 1)) + chooseMain];
            sideSphere[(chooseMain / (width - 1)) + chooseMain] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width] = sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + width + 1] = sideSphere[(chooseMain / (width - 1)) + chooseMain + 1];
            sideSphere[(chooseMain / (width - 1)) + chooseMain + 1] = tmpObj;

            ////スクリプト入れ替え
            tmpAnim = panelAnim[(chooseMain / (width - 1)) + chooseMain];
            panelAnim[(chooseMain / (width - 1)) + chooseMain] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width] = panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + width + 1] = panelAnim[(chooseMain / (width - 1)) + chooseMain + 1];
            panelAnim[(chooseMain / (width - 1)) + chooseMain + 1] = tmpAnim;

            panelMove[1] = false;
            //    }
            //    changeTime = 0.1f;
        }
    }

    void ScoreAdd()
    {

        ////スコア100と1コンボ50
        score += 100 + (50 * ComboCS.comboCount);

        scoreText.text = "" + score;

    }

    void TurnEnd()
    {

        if (TimerCS.timeCount <= 0f) //制限時間でゲーム終了
        {
            if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
            Result();   //リザルトに遷移
        }
    }
    void PointBlinking()
    {
        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {

                judgNum = mainNumber[i];

                if (judgNum == sideNumber[(i / (width - 1)) + i] * 4 || sideNumber[(i / (width - 1)) + i] == 32)
                    if (judgNum == sideNumber[(i / (width - 1)) + i + 1] * 4 || sideNumber[(i / (width - 1)) + i + 1] == 32)
                        if (judgNum == sideNumber[(i / (width - 1)) + i + width + 1] * 4 || sideNumber[(i / (width - 1)) + i + width + 1] == 32)
                            if (judgNum == sideNumber[(i / (width - 1)) + i + width] * 4 || sideNumber[(i / (width - 1)) + i + width] == 32) //色を満たした
                            {
                                float blinking = 0f;
                                float blinkingSpeed = 2.0f;
                                blinking = Mathf.Sin(2 * Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅(-1~1)

                                //mainSphere[i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);草
                                sideSphere[(i / (width - 1)) + i].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                sideSphere[(i / (width - 1)) + i + width + 1].GetComponent<Renderer>().material.SetFloat("_AtmosphereDensity", 6 + blinking);
                                //Debug.Log(blinking);
                            }
                judgNum = 0;
            }
        }
    }

    void ExplosionStop()
    {
        for (int i = 0; i < mainPanel; i++)
        {
            ExplosionCS.particle[i].Stop();
        }
    }

    void MainGenerate()
    {

        do
        {
            //ランダムな数値にいれかえ
            mainNumber[0] = mainColorNumber[Random.Range(0, 4)];
            checkColorNum = 0;

            for (int i = 0; i < sidePanel; i++)
            {

                if (sideNumber[i] * 4 == mainNumber[0])
                {
                    checkColorNum += 1;
                }
            }
        } while (checkColorNum < 4);
    }

    void Result()
    {
        //リザルト画面へ
        oldSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("testresurt");
    }

    void ClearCheck()
    {
        
        if ( targetNum[0] <= 0 )
        {
            Debug.Log(ComboCS.comboCount);
            TimerCS.timeCount += 4.0f * ComboCS.comboCount;
            if (ComboCS.comboCount > 1) TimerCS.timeCount += 2.0f;
            targetNum[0] = 3;
            int i;
            do
            {
                i = Random.Range(0, 4);
            } while (mainNumber[0] == mainColorNumber[i]);
            mainNumber[0] = mainColorNumber[i];
        }
        ColorChange();
    }
}
