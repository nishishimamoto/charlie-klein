using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Isha_Singlshot : MonoBehaviour
{
    [SerializeField] Cursor cursorSelectCS;
    [SerializeField] StartFade StartFadeCS;
    [SerializeField] test3timer TimerCS;
    [SerializeField] Combo ComboCS;
    [SerializeField] Explosion ExplosionCS;
    [SerializeField] Pause PauseCS;
    [SerializeField] Image BonusGauge;
    [SerializeField] Image BonusGaugeOut;
    [SerializeField] Image BonusGaugeSand;
    [SerializeField] Image BonusGaugeSandOut;
    [SerializeField] GameSE gameSECS;

    [SerializeField] Image Needle;
    float needleAngle;

    const int mainPanel = 30;    //メインパネルの数
    const int sidePanel = 42;    //サイドパネルの数
    const int width = 7;   //横の数
    const int height = 6;    //縦の数

    int HeavensTime = 0;

    int[] mainNumber = new int[mainPanel];     //3*3のナンバー
    int[] sideNumber = new int[sidePanel];     //4*4のナンバー
    int tmpNumber;          //数字入れ替え時の一時保存
    int[] bonusLevel = new int[sidePanel]; //サイドパネルのボーナス確認 (0=なし,1=あり)
    public int BonusFlg = 0; //ボーナスタイム発動したか否か
    int BsCnt = 0;
    float BonusTime = 0;
    float BonusAccel;
    int AccelCnt;
    int tmpBonus;         //ボーナス入れ替え時の一時保存
    public static int score;      //スコア

    int chooseMain = -1; //現在選んでいるメインナンバー

    bool isHorizontal;     //十字キーの左右入力がニュートラルにもどったか
    bool isVertical;    //十字キーの上下入力がニュートラルにもどったか

    bool isOperation;
    bool isStart;

    bool isDebug;

    bool chargeSE;
    bool overCS;

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

    GameObject[] chainPos = new GameObject[30];

    int[] ColorNumber = { 1, 2, 3, 4 };    //メイン色の配列(赤、青、緑、黄)
    int[] sideColorNumber = { 1, 8, 32, 128 };     //サイド色の配列(赤、青、緑、黄)
    int[] rainbowNumber = { 0, 1, width, width + 1 };           //虹衛星を出すときに使う
    bool rainbow;
    int[] rainbowRand = new int[mainPanel]; //虹衛星をランダムに配置するための変数
    int rainbowTarget = 0;
    //[SerializeField] GameObject selectMainImage; //現在選択しているメインパネルを表示

    [SerializeField] GameObject Score;  //スコアのテキストオブジェクト
    Text scoreText;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject BonusText;
    [SerializeField] GameObject TimeUpText;
    [SerializeField] GameObject TheWorld;
    [SerializeField] GameObject TimeUpBack;
    [SerializeField] GameObject DebugText;
    [SerializeField] GameObject ListText;

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
    [SerializeField] GameObject chain;
    [SerializeField] bool[] isPlanet = new bool[mainPanel];

    public static string oldSceneName;  //リザルトから戻る用

    bool isAnyAnim;
    float animTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        BonusGauge.fillAmount = 0f;
        BonusGaugeOut.fillAmount = 0f;
        BonusGaugeSand.fillAmount = 0f;
        BonusGaugeSandOut.fillAmount = 0f;
        BonusFlg = 0;
        BonusTime = 10;
        BonusAccel = 0.1f;
        AccelCnt = 10;
        BonusText.gameObject.SetActive(false);

        BsCnt = 1;

        Needle.gameObject.SetActive(false);
        needleAngle = 0;

        Pause.gameObject.SetActive(true);
        TheWorld.gameObject.SetActive(false);
        TimeUpText.gameObject.SetActive(false);
        TimeUpBack.gameObject.SetActive(false);
        DebugText.gameObject.SetActive(false);
        ListText.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    //左上
                    mainSphere[i] = Instantiate(main, new Vector3(7.125f, 3.625f, 0), Quaternion.identity);
                    break;
                case 1:
                    //右上
                    mainSphere[i] = Instantiate(main, new Vector3(8.875f, 3.625f, 0), Quaternion.identity);
                    break;
                case 2:
                    //左下
                    mainSphere[i] = Instantiate(main, new Vector3(7.125f, 1.875f, 0), Quaternion.identity);
                    break;
                case 3:
                    //右下
                    mainSphere[i] = Instantiate(main, new Vector3(8.875f, 1.875f, 0), Quaternion.identity);
                    break;
                //case 4:
                //    //次の次　左下
                //    mainSphere[i] = Instantiate(main, new Vector3(8f, -2f, 0), Quaternion.identity);
                //    break;
                //case 5:
                //    //次の次　左下
                //    mainSphere[i] = Instantiate(main, new Vector3(9f, -2f, 0), Quaternion.identity);
                //    break;
                //case 6:
                //    //次の次　左下
                //    mainSphere[i] = Instantiate(main, new Vector3(8f, -3f, 0), Quaternion.identity);
                //    break;
                //case 7:
                //    //次の次　左下
                //    mainSphere[i] = Instantiate(main, new Vector3(9f, -3f, 0), Quaternion.identity);
                //    break;

            }
            mainNumber[i] = ColorNumber[Random.Range(1, 4)];
        }

        for (int i = 0; i < 30; i++)
        {
            chainPos[i] = Instantiate(chain, new Vector3(-6.5f + (2 * (i % 6)), 4f - (2 * (i / 6)), 2.0f), Quaternion.identity);
            chainPos[i].gameObject.SetActive(false);
        }

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
                    sideSphere[i] = Instantiate(side, new Vector3(-5.5f + (2 * (i % width - 1)), 5f - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = ColorNumber[Random.Range(0, 4)];
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
                    sideSphere[i] = Instantiate(side, new Vector3(-5.5f + (2 * (i % width - 1)), 5f - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = ColorNumber[Random.Range(0, 4)];
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
                    sideSphere[i] = Instantiate(side, new Vector3(-5.5f + (2 * (i % width - 1)), 5f - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = ColorNumber[Random.Range(0, 4)];
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
                    sideSphere[i] = Instantiate(side, new Vector3(-5.5f + (2 * (i % width - 1)), 5f - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = ColorNumber[Random.Range(0, 4)];
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
                    sideSphere[i] = Instantiate(side, new Vector3(-5.5f + (2 * (i % width - 1)), 5f - (2 * (i / width)), 0.0f), Quaternion.identity);
                    panelAnim[i] = sideSphere[i].GetComponent<PanelAnim>();
                    sideNumber[i] = ColorNumber[Random.Range(0, 4)];

                }
            }
       }

        //生成された衛星が揃っていないか確認
        for (int i = 0; i < mainPanel; i++)
        {
            if (!isPlanet[i])
            {
                if (i >= 1)
                {
                    //左が右と同じ色かチェック
                    if (sideNumber[(i / (width - 1)) + i] == sideNumber[(i / (width - 1)) + i + 1])
                    {
                        int numSet = 0;
                        int numUpSet = 0;
                        int randSet = ColorNumber[Random.Range(0, 4)];
                        
                        //左が上と同じ色かチェック
                        if (i >= 6 && sideNumber[(i / (width - 1)) + i] == sideNumber[(i / (width - 1)) + i - width])
                        {
                            numSet = sideNumber[(i / (width - 1)) + i - 1];
                            numUpSet = sideNumber[(i / (width - 1)) + i - width];
                            sideNumber[(i / (width - 1)) + i] = ColorNumber[Random.Range(0, 4)];
                            //左の色の変更
                            while (numSet == sideNumber[(i / (width - 1)) + i] ||
                                sideNumber[(i / (width - 1)) + i] == sideNumber[(i / (width - 1)) + i - width])
                            {
                                sideNumber[(i / (width - 1)) + i] = ColorNumber[Random.Range(0, 4)];
                            }
                        }
                        else
                        {
                            numSet = sideNumber[(i / (width - 1)) + i - 1];
                            sideNumber[(i / (width - 1)) + i] = ColorNumber[Random.Range(0, 4)];
                            //左の色の変更
                            while (numSet == sideNumber[(i / (width - 1)) + i])
                            {
                                sideNumber[(i / (width - 1)) + i] = ColorNumber[Random.Range(0, 4)];
                            }
                        }
                    }
                }
            }
        }

        scoreText = Score.GetComponent<Text>();

        ColorChange();   //パネルの色変更
        TimerCS.maxTime = 60.0f;
        TimerCS.timeCount = TimerCS.maxTime;
        score = 0;  //スコアの初期化


    }

    // Update is called once per frame
    void Update()
    {
        
        if (!PauseCS.isPause)
        {
            if (!StartFadeCS.isTurnStart)
            {
                StartFadeCS.StartFadeOut();
                if (!TimerCS.countStart) TimerCS.countStart = true;
            }
            else if(StartFadeCS.isTurnStart)
            {
                if (!isOperation)
                {
                    if (!panelMove[0] && !panelMove[1]) PanelOperation();   //パネルの操作
                    else if (panelMove[0] || panelMove[1]) PanelMove();        //パネルのアニメーション
                }

                if (HeavensTime == 0)
                {
                    PointCheck();             //盤面が揃ったか見る 揃ったらすぐ変わる
                    TimerCS.TimerCount();       //制限時間のカウントと表示
                }

                TurnEnd();                  //ターン終了時の処理


                cursorSelectCS.SelectImageMove(chooseMain);

                if (!alpha_Flg || BonusFlg == 1)
                {
                    Bonus();
                }
                else if (alpha_Flg)
                {
                    alpha();
                }
            }
        }
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

                //新しい衛星の生成
                //左上
                sideNumber[(check / (width - 1)) + check] = mainNumber[0];
                //右上
                sideNumber[(check / (width - 1)) + check + 1] = mainNumber[1];
                //左下
                sideNumber[(check / (width - 1)) + check + width] = mainNumber[2];
                //右下
                sideNumber[(check / (width - 1)) + check + width + 1] = mainNumber[3];


                rainbowRand[rainbowTarget] = check; //条件を満たした惑星の位置を把握しておく
                rainbowTarget += 1;

                ScoreAdd();

                gameSECS.audioSource.PlayOneShot(gameSECS.explosion2SE);
                ExplosionCS.particle[check].Play(); //条件を満たした惑星が爆発
                Invoke("ExplosionStop", 1.0f);    //時間差で爆発を止める
                //ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生

                for (int i = 0; i < 4; i++)
                {
                    mainNumber[i] = ColorNumber[Random.Range(0, 4)];
                }

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


                for (int f = 0; f < mainPanel; f++) flgCheck[f] = false; //念のため別のforでfalseにする
                mainColorNum = 0;
                ColorChange();   //パネルの色変更
                check = 0;
                rainbow = false;
                alpha_Flg = false;
                if (BsCnt == 1)
                {
                    BonusGauge.fillAmount += BonusAccel * ComboCS.comboCount;
                    BonusGaugeOut.fillAmount += BonusAccel * ComboCS.comboCount;
                    BonusGaugeSand.fillAmount += BonusAccel * ComboCS.comboCount;
                }
                ComboCS.comboCount = 0;
                rainbowTarget = 0;
            }
        }
        else check += 1;
    }
    //***
    void PanelOperation()
    {
        if (isDebug == false) // デバッグモードoff中
        {
            if (Input.GetButtonDown("Back")) //デバッグモードon
            {
                DebugText.gameObject.SetActive(true);
                ListText.gameObject.SetActive(true);
                isDebug = true;

            }
        }
        else //デバッグモードon中
        {
            if (Input.GetKeyDown("t")) //時間停止
            {
                if (HeavensTime == 0)
                {
                    HeavensTime = 1;
                }
                else
                {
                    HeavensTime = 0;
                }
            }
            if (Input.GetKeyDown("g")) //ゲージマックス
            {
                BonusGauge.fillAmount = 1f;
                BonusGaugeOut.fillAmount = 1f;
                BonusGaugeSand.fillAmount = 1f;
            }
            if (Input.GetKeyDown("f")) //残り10秒にする
            {
                TimerCS.timeCount = 10.0f;
            }
            if (Input.GetKeyDown("p")) //残り10秒にする
            {
                TimerCS.timeCount += 10.0f;
            }
            if (Input.GetButtonDown("Back"))//デバッグモードoff
            {
                DebugText.gameObject.SetActive(false);
                ListText.gameObject.SetActive(false);
                isDebug = false;
            }


        }


        //確変
        if (Input.GetButtonDown("B") && BonusGaugeSand.fillAmount >= 1)
        {
            gameSECS.audioSource.Stop();
            BonusFlg = 1;
            TheWorld.gameObject.SetActive(true);
        }

        ////パネル反時計回り
        //if (Input.GetButtonDown("LB"))
        //{
        //    panelMove[0] = true;
        //    //if (!TimerCS.countStart) TimerCS.countStart = true;
        //    //ComboCS.comboCount = 0;
        //}
        ////パネル時計回り
        //else if (Input.GetButtonDown("RB"))
        //{
        //    panelMove[1] = true;
        //    //if (!TimerCS.countStart) TimerCS.countStart = true;
        //}

        if (!isAnyAnim)
        {
            //パネル反時計回り
            if (Input.GetButtonDown("LB"))
            {
                gameSECS.audioSource.PlayOneShot(gameSECS.spinSE);
                isAnyAnim = true;
                panelMove[0] = true;
            }
            //パネル時計回り
            else if (Input.GetButtonDown("RB"))
            {
                gameSECS.audioSource.PlayOneShot(gameSECS.spinSE);
                isAnyAnim = true;
                panelMove[1] = true;
            }
        }
        else if (isAnyAnim)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 0.1f)
            {
                isAnyAnim = false;
                animTimer = 0;
            }
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

                if (sideNumber[(i / (width - 1)) + i] == sideNumber[(i / (width - 1)) + i + 1])
                {
                    if (sideNumber[(i / (width - 1)) + i + 1] == sideNumber[(i / (width - 1)) + i + width + 1])
                    {
                        if (sideNumber[(i / (width - 1)) + i + width + 1] == sideNumber[(i / (width - 1)) + i + width])
                        {

                            flgCheck[i] = true;
                            alpha_Flg = true;
                            if (BonusFlg == 1)
                            {
                                if(chainPos[i].activeSelf == false)
                                {
                                    Debug.Log("aaa");
                                    gameSECS.audioSource.PlayOneShot(gameSECS.massSE);
                                }
                                chainPos[i].gameObject.SetActive(true);
                                switch (sideNumber[(i / (width - 1)) + i])
                                {
                                    case 1:
                                        chainPos[i].GetComponent<SpriteRenderer>().color = Color.blue;
                                        break;
                                    case 2:
                                        chainPos[i].GetComponent<SpriteRenderer>().color = Color.red;
                                        break;
                                    case 3:
                                        chainPos[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                                        break;
                                    case 4:
                                        chainPos[i].GetComponent<SpriteRenderer>().color = Color.cyan;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {

                                chainPos[i].gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            chainPos[i].gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        chainPos[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    chainPos[i].gameObject.SetActive(false);
                }

            }
        }
    }

    //***
    public void ColorChange()
    {

        for(int i = 0; i < 4; i++)
        {
            switch (mainNumber[i])
            {
                case 1:
                    mainSphere[i].GetComponent<Renderer>().material = _material[0];
                    break;
                case 2:
                    mainSphere[i].GetComponent<Renderer>().material = _material[1];
                    break;
                case 3:
                    mainSphere[i].GetComponent<Renderer>().material = _material[2];
                    break;
                case 4:
                    mainSphere[i].GetComponent<Renderer>().material = _material[3];
                    break;
            }
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
                case 2:
                    //sideSphereColor[i] = Color.blue;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[1];
                    break;
                case 3:
                    //sideSphereColor[i] = Color.yellow;
                    //sideSphere[i].GetComponent<Renderer>().material.color = sideSphereColor[i];
                    sideSphere[i].GetComponent<Renderer>().material = _material[2];
                    break;
                case 4:
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
        if (BonusGaugeSand.fillAmount >= 1)
        {
            BonusText.gameObject.SetActive(true);
            if (chargeSE == false)
            {
                gameSECS.audioSource.PlayOneShot(gameSECS.bomChargeSE);
                chargeSE = true;
            }
        }
        else
        {
            BonusText.gameObject.SetActive(false);
        }

        if (BonusFlg == 1)
        {
            //gameSECS.audioSource.Stop();
            TimerCS.countStart = false;
            TimerCS.bigTimerText.enabled = false;

            BonusGaugeSandOut.fillAmount = 1f;
            BsCnt = 0;
            Needle.gameObject.SetActive(true);
            needleAngle = -36 * Time.deltaTime;
            Needle.transform.Rotate(new Vector3(0f, 0f, needleAngle));

            BonusGauge.fillAmount = BonusTime / 10;
            BonusGaugeOut.fillAmount = BonusTime / 10;
            BonusGaugeSand.fillAmount = BonusTime / 10;
            BonusTime -= Time.deltaTime;

            if (BonusGaugeSand.fillAmount <= 0)
            {
                alpha();
                BsCnt = 1;
                TimerCS.countStart = true;
                chargeSE = false;
                Needle.gameObject.SetActive(false);

                //switch (BonusAccel)
                //{
                //    case 0.1f: //10回
                //        BonusAccel = 0.084f;
                //        break; //12回
                //    case 0.084f:
                //        BonusAccel = 0.067f;
                //        break;
                //    case 0.067f:
                //        BonusAccel = 0.05f;
                //        break;
                //    case 0.05f:
                //        if (AccelCnt > 4)
                //        {
                //            BonusAccel = 0.04f;
                //        }
                //        AccelCnt += 1;
                //        break;
                //}

                AccelCnt += 2;
                BonusAccel = 1.0f / AccelCnt;

                TheWorld.gameObject.SetActive(false);
                BonusTime = 10;
                BonusGauge.fillAmount = 0;
                BonusGaugeOut.fillAmount = 0;
                BonusGaugeSand.fillAmount = 0;
                BonusGaugeSandOut.fillAmount = 0;
                BonusFlg = 0;
            }
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
        }
    }

    void ScoreAdd()
    {

        ////スコア100と1コンボ50
        score += 100 + (50 * ComboCS.comboCount);
        if (BsCnt == 0)
        {
            score += 100;
        }
        scoreText.text = "" + score;

    }

    void TurnEnd()
    {

        if (TimerCS.timeCount <= 5 && !gameSECS.is5countSE)
        {
            gameSECS.audioSource.PlayOneShot(gameSECS.timerSE);
            
            gameSECS.is5countSE = true;
        }
        else if(TimerCS.timeCount > 5)
        {
            gameSECS.is5countSE = false;
        }

        if (TimerCS.timeCount <= 0f && overCS == false) //制限時間でゲーム終了
        {
            TimeUpBack.gameObject.SetActive(true);
            TimeUpText.gameObject.SetActive(true);

            isOperation = true;
            overCS = true;
            gameSECS.audioSource.PlayOneShot(gameSECS.gameOverSE);
            if (TimerCS.timeCount > 0) TimerCS.timeCount = 0f;
            TimerCS.countStart = false;
            Invoke("Result", 3.0f);  //リザルトに遷移
        }
    }

    void ExplosionStop()
    {
        for (int i = 0; i < mainPanel; i++)
        {
            ExplosionCS.particle[i].Stop();
        }
        //ExplosionCS.audio.PlayOneShot(ExplosionCS.clip);//爆発のSEを再生
    }

    void Result()
    {
        //リザルト画面へ
        oldSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MasterResult");
    }

    void ClearCheck()
    {
        
        TimerCS.timeCount += 1.5f * ComboCS.comboCount;
        ComboCS.comboCount += 1;
        //Debug.Log(ComboCS.comboCount);
        ColorChange();
    }
}
