using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawLine
{
	/// <summary>
	/// ゲームメインクラス
	/// </summary>
	public class Game1 : Game
	{
    /// <summary>
    /// グラフィックデバイス管理クラス
    /// </summary>
    private readonly GraphicsDeviceManager _graphics = null;

    /// <summary>
    /// スプライトのバッチ化クラス
    /// </summary>
    private SpriteBatch _spriteBatch = null;

    /// <summary>
    /// LineList 用頂点バッファ
    /// </summary>
    private VertexBuffer _lineListVertexBuffer = null;

    /// <summary>
    /// LineStrip 用頂点バッファ
    /// </summary>
    private VertexBuffer _lineStripVertexBuffer = null;

    /// <summary>
    /// 基本エフェクト
    /// </summary>
    private BasicEffect _basicEffect = null;


    /// <summary>
    /// GameMain コンストラクタ
    /// </summary>
    public Game1()
    {
      // グラフィックデバイス管理クラスの作成
      _graphics = new GraphicsDeviceManager(this);

      // ゲームコンテンツのルートディレクトリを設定
      Content.RootDirectory = "Content";

      // マウスカーソルを表示
      IsMouseVisible = true;
    }

    /// <summary>
    /// ゲームが始まる前の初期化処理を行うメソッド
    /// グラフィック以外のデータの読み込み、コンポーネントの初期化を行う
    /// </summary>
    protected override void Initialize()
    {
      // TODO: ここに初期化ロジックを書いてください

      // コンポーネントの初期化などを行います
      base.Initialize();
    }

    /// <summary>
    /// ゲームが始まるときに一回だけ呼ばれ
    /// すべてのゲームコンテンツを読み込みます
    /// </summary>
    protected override void LoadContent()
    {
      // テクスチャーを描画するためのスプライトバッチクラスを作成します
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      // エフェクトを作成
      _basicEffect = new BasicEffect(GraphicsDevice);

      // エフェクトで頂点カラーを有効にする
      _basicEffect.VertexColorEnabled = true;

      // ビューマトリックスをあらかじめ設定 ((0, 0, 15) から原点を見る)
      _basicEffect.View = Matrix.CreateLookAt(
              new Vector3(0.0f, 0.0f, 15.0f),
              Vector3.Zero,
              Vector3.Up
          );

      // プロジェクションマトリックスをあらかじめ設定
      _basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
              MathHelper.ToRadians(45.0f),
              (float)GraphicsDevice.Viewport.Width /
                  (float)GraphicsDevice.Viewport.Height,
              1.0f,
              100.0f
          );

      // 頂点の数
      int vertexCount = 6;

      // LineList 用頂点バッファ作成
      _lineListVertexBuffer = new VertexBuffer(GraphicsDevice,
          typeof(VertexPositionColor), vertexCount, BufferUsage.None);

      // LineList 用頂点データを作成する
      VertexPositionColor[] lineListVertices = new VertexPositionColor[vertexCount];

      lineListVertices[0] = new VertexPositionColor(new Vector3(-2.5f, 2.5f, 0.0f),
                                                    Color.Red);
      lineListVertices[1] = new VertexPositionColor(new Vector3(-1.5f, 0.5f, 0.0f),
                                                    Color.Blue);
      lineListVertices[2] = new VertexPositionColor(new Vector3(-0.5f, 2.5f, 0.0f),
                                                    Color.Green);
      lineListVertices[3] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.0f),
                                                    Color.Yellow);
      lineListVertices[4] = new VertexPositionColor(new Vector3(1.5f, 2.5f, 0.0f),
                                                    Color.Black);
      lineListVertices[5] = new VertexPositionColor(new Vector3(2.5f, 0.5f, 0.0f),
                                                    Color.White);

      // 頂点データを頂点バッファに書き込む
      _lineListVertexBuffer.SetData(lineListVertices);

      // LineStrip 用頂点バッファ作成
      _lineStripVertexBuffer = new VertexBuffer(GraphicsDevice,
          typeof(VertexPositionColor), vertexCount, BufferUsage.None);

      // LineStrip 用頂点データを作成する
      VertexPositionColor[] lineStripVertices = new VertexPositionColor[vertexCount];

      lineStripVertices[0] = new VertexPositionColor(new Vector3(-2.5f, -0.5f, 0.0f),
                                                     Color.Red);
      lineStripVertices[1] = new VertexPositionColor(new Vector3(-1.5f, -2.5f, 0.0f),
                                                     Color.Blue);
      lineStripVertices[2] = new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.0f),
                                                     Color.Green);
      lineStripVertices[3] = new VertexPositionColor(new Vector3(0.5f, -2.5f, 0.0f),
                                                     Color.Yellow);
      lineStripVertices[4] = new VertexPositionColor(new Vector3(1.5f, -0.5f, 0.0f),
                                                     Color.Black);
      lineStripVertices[5] = new VertexPositionColor(new Vector3(2.5f, -2.5f, 0.0f),
                                                     Color.White);

      // 頂点データを頂点バッファに書き込む
      _lineStripVertexBuffer.SetData(lineStripVertices);
    }

    /// <summary>
    /// ゲームが終了するときに一回だけ呼ばれ
    /// すべてのゲームコンテンツをアンロードします
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: ContentManager で管理されていないコンテンツを
      //       ここでアンロードしてください
    }

    /// <summary>
    /// 描画以外のデータ更新等の処理を行うメソッド
    /// 主に入力処理、衝突判定などの物理計算、オーディオの再生など
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Update(GameTime gameTime)
    {
      // ゲームパッドの Back ボタン、またはキーボードの Esc キーを押したときにゲームを終了させます
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }

      // TODO: ここに更新処理を記述してください

      // 登録された GameComponent を更新する
      base.Update(gameTime);
    }

    /// <summary>
    /// 描画処理を行うメソッド
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Draw(GameTime gameTime)
    {
      // 画面を指定した色でクリアします
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // 描画に使用する頂点バッファをセットします
      GraphicsDevice.SetVertexBuffer(_lineListVertexBuffer);

      // パスの数だけ繰り替えし描画
      foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
      {
        // パスの開始
        pass.Apply();

        // LineList で描画する
        GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 3);
      }

      // 描画に使用する頂点バッファをセットします
      GraphicsDevice.SetVertexBuffer(_lineStripVertexBuffer);

      // パスの数だけ繰り替えし描画
      foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
      {
        // パスの開始
        pass.Apply();

        // LineStrip で描画する
        GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, 5);
      }

      // 登録された DrawableGameComponent を描画する
      base.Draw(gameTime);
    }
  }
}
