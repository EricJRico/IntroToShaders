# How to Use
The **Assets/0_Start** is starting the point for each shader. Using ShaderGraph and the mermaid diagrams below, create each shader below.

The session is meant to be a "Watch -> Do it yourself -> Discuss" workflow for each shader.

The duration is lengthened to allow for more time for those new to shaders and ShaderGraph. There are also challenges for each section as shown below for ones that finish early.

# Intro to Shaders Workshop — Build Reference

## Time Breakdown

| Time | Duration | Content |
|------|----------|---------|
| 0:00–0:15 | 15 min | Welcome & Setup |
| 0:15–0:45 | 30 min | **Shader 1:** Color Tint |
| 0:45–1:15 | 30 min | **Shader 2:** Snow Accumulation |
| 1:15–1:25 | 10 min | Break |
| 1:25–1:35 | 10 min | **Bonus:** Damage Mask |
| 1:35–2:10 | 35 min | **Shader 3:** Scrolling Water |
| 2:10–2:45 | 35 min | **Shader 4:** Dissolve Effect |
| 2:45–2:55 | 10 min | **Shader 5:** Rim Highlight *(stretch)* |
| 2:55–3:00 | 5 min | Wrap-Up & Q&A |

---

## Shader 1: Color Tint (30 min)

**Use cases:** Damage feedback, status effects, team colors

```mermaid
flowchart LR
    subgraph Properties
        MT["Texture2D<br/>Main Texture"]
        TC["Color<br/>Tint Color"]
        TS["Float 0-1<br/>Tint Strength"]
    end

    MT --> ST[Sample Texture 2D]
    ST --> MUL[Multiply]
    TC --> MUL
    
    ST --"A"--> LERP[Lerp]
    MUL --"B"--> LERP
    TS --"T"--> LERP
    
    LERP --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Create Unlit Shader Graph
2. Add properties: Main Texture (Texture2D), Tint Color (Color), Tint Strength (Float, 0-1)
3. Sample Texture 2D → Multiply with Tint Color
4. Lerp: A = Sampled Texture, B = Multiplied result, T = Tint Strength
5. Output to Base Color

**Stretch Challenge:** Add second Color property "Alt Tint" and a Tint Blend slider to control which color is used.

```mermaid
flowchart LR
    subgraph Properties
        MT["Texture2D<br/>Main Texture"]
        TC["Color<br/>Tint Color"]
        AT["Color<br/>Alt Tint"]
        TB["Float 0-1<br/>Tint Blend"]
        TS["Float 0-1<br/>Tint Strength"]
    end

    MT --> ST[Sample Texture 2D]
    
    TC --"A"--> LERP1[Lerp]
    AT --"B"--> LERP1
    TB --"T"--> LERP1
    
    ST --> MUL[Multiply]
    LERP1 --> MUL
    
    ST --"A"--> LERP2[Lerp]
    MUL --"B"--> LERP2
    TS --"T"--> LERP2
    
    LERP2 --> BC["Base Color<br/>(Master Stack)"]
```

---

## Shader 2: Snow Accumulation (30 min)

**Use cases:** Weather systems, environment variation, moss/dust effects

```mermaid
flowchart LR
    subgraph Properties
        BT["Texture2D<br/>Base Texture"]
        SNT["Texture2D<br/>Snow Texture"]
        SA["Float 0-1<br/>Snow Amount"]
        BA["Float<br/>Blend Amount"]
    end

    subgraph "World Direction"
        NV["Normal Vector<br/>(World)"]
        UP["Vector3<br/>(0, 1, 0)"]
    end

    BT --> STB[Sample Texture 2D]
    SNT --> STS[Sample Texture 2D]
    
    NV --> DOT[Dot Product]
    UP --> DOT
    
    SA --> OM[One Minus]
    OM --"Edge1"--> SS[Smoothstep]
    OM --> ADD[Add]
    BA --> ADD
    ADD --"Edge2"--> SS
    DOT --"In"--> SS
    
    STB --"A"--> LERP[Lerp]
    STS --"B"--> LERP
    SS --"T"--> LERP
    
    LERP --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Create Unlit Shader Graph
2. Add properties: Base Texture, Snow Texture (Texture2D), Snow Amount (Float, 0-1), Blend Amount (Float, default ~0.1)
3. Sample both textures
4. Normal Vector (World) → Dot Product with Vector3 (0, 1, 0)
5. One Minus on Snow Amount
6. Smoothstep: Edge1 = One Minus, Edge2 = One Minus + Blend Amount, In = Dot Product
7. Lerp: A = Base Texture, B = Snow Texture, T = Smoothstep
8. Output to Base Color

**Stretch Challenge — Angled Snow:** Add a Snow Direction property to control wind-blown snow.

```mermaid
flowchart LR
    subgraph Properties
        SD["Vector3<br/>Snow Direction<br/>(0.3, 1, 0)"]
    end
    
    SD --> NORM[Normalize]
    
    NV["Normal Vector<br/>(World)"] --> DOT[Dot Product]
    NORM --> DOT
    
    DOT --> REST["...rest of shader"]
```

---

## Bonus: Damage Mask (10 min)

**Use cases:** Artistic damage control, wear patterns, painted effects

```mermaid
flowchart LR
    subgraph Properties
        BT["Texture2D<br/>Base Texture"]
        DT["Texture2D<br/>Damage Texture"]
        DM["Texture2D<br/>Damage Mask"]
        DA["Float 0-1<br/>Damage Amount"]
    end

    BT --> STB[Sample Texture 2D]
    DT --> STD[Sample Texture 2D]
    DM --> STM[Sample Texture 2D]
    
    STM --> MUL[Multiply]
    DA --> MUL
    
    STB --"A"--> LERP[Lerp]
    STD --"B"--> LERP
    MUL --"T"--> LERP
    
    LERP --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Extend Color Tint shader (or create new)
2. Add properties: Base Texture, Damage Texture, Damage Mask (Texture2D), Damage Amount (Float, 0-1)
3. Sample all three textures
4. Multiply: Mask × Damage Amount
5. Lerp: A = Base Texture, B = Damage Texture, T = Multiply result
6. Output to Base Color

---

## Shader 3: Scrolling Water (35 min)

**Use cases:** Rivers, lava, conveyor belts, energy effects

```mermaid
flowchart LR
    subgraph Properties
        WT["Texture2D<br/>Water Texture<br/>(Tiling enabled)"]
        SS["Vector2<br/>Scroll Speed"]
    end

    POS["Position<br/>(World)"] --> SW["Swizzle XZ"]
    
    TIME[Time] --> MUL[Multiply]
    SS --> MUL
    
    SW --> ADD[Add]
    MUL --> ADD
    
    ADD --> ST["Sample Texture 2D<br/>(UV input)"]
    WT --> ST
    
    ST --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Create Unlit Shader Graph
2. Add properties: Water Texture (Texture2D, enable Tiling), Scroll Speed (Vector2, default 0.1, 0.05)
3. Position (World) → Swizzle to XZ
4. Time × Scroll Speed
5. Add: Position XZ + Time result
6. Connect Add to Sample Texture 2D UV input
7. Output to Base Color

**Stretch Challenge A — Separate Scroll Speeds:**

```mermaid
flowchart LR
    subgraph Properties
        SSX["Float<br/>Scroll Speed X"]
        SSY["Float<br/>Scroll Speed Y"]
    end

    TIME[Time] --> MUL1[Multiply]
    SSX --> MUL1
    
    TIME --> MUL2[Multiply]
    SSY --> MUL2
    
    MUL1 --> COMB["Combine<br/>(Vector2)"]
    MUL2 --> COMB
    
    COMB --> ADD["Add<br/>(to Position XZ)"]
```

**Stretch Challenge B — Foam Overlay Layer:**

```mermaid
flowchart LR
    subgraph Properties
        WT["Texture2D<br/>Water Texture"]
        FT["Texture2D<br/>Foam Texture"]
        WS["Vector2<br/>Water Speed"]
        FS["Vector2<br/>Foam Speed"]
    end

    POS["Position XZ"]
    TIME[Time]
    
    TIME --> MUL1[Multiply]
    WS --> MUL1
    MUL1 --> TO1["Tiling And Offset"]
    POS --> TO1
    TO1 --> ST1[Sample Texture 2D]
    WT --> ST1
    
    TIME --> MUL2[Multiply]
    FS --> MUL2
    MUL2 --> TO2["Tiling And Offset"]
    POS --> TO2
    TO2 --> ST2[Sample Texture 2D]
    FT --> ST2
    
    ST1 --"Background"--> BLEND["Blend<br/>(Overlay)"]
    ST2 --"Blend"--> BLEND
    ST2 --"Alpha → Opacity"--> BLEND
    
    BLEND --> BC["Base Color"]
```

---

## Shader 4: Dissolve Effect (35 min)

**Use cases:** Spawn/despawn, teleportation, death effects, magic

```mermaid
flowchart LR
    subgraph Properties
        MT["Texture2D<br/>Main Texture"]
        DA["Float 0-1<br/>Dissolve Amount"]
        EW["Float<br/>Edge Width"]
        EC["Color HDR<br/>Edge Color"]
    end

    MT --> ST[Sample Texture 2D]
    NOISE["Simple Noise<br/>(Scale ~20)"]
    
    DA --"Edge"--> STEP[Step]
    NOISE --"In"--> STEP
    
    STEP --> ALPHA["Alpha<br/>(Master Stack)"]
    
    DA --"Edge1"--> SMOOTH[Smoothstep]
    DA --> ADD2[Add]
    EW --> ADD2
    ADD2 --"Edge2"--> SMOOTH
    NOISE --"In"--> SMOOTH
    
    SMOOTH --> OM[One Minus]
    OM --> MUL[Multiply]
    EC --> MUL
    
    ST --> ADD3[Add]
    MUL --> ADD3
    
    ADD3 --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Create Unlit Shader Graph, enable Alpha Clipping in Graph Inspector
2. Add properties: Main Texture (Texture2D), Dissolve Amount (Float, 0-1), Edge Width (Float, ~0.05), Edge Color (Color, HDR)
3. Sample Main Texture
4. Simple Noise (Scale ~20)
5. Step: Edge = Dissolve Amount, In = Noise → Alpha output
6. Smoothstep: Edge1 = Dissolve Amount, Edge2 = Dissolve Amount + Edge Width, In = Noise
7. One Minus → Multiply with Edge Color
8. Add glow to sampled texture → Base Color output

**Stretch Challenge A — Animate with Sine:**

```mermaid
flowchart LR
    TIME[Time] --> SIN[Sine]
    SIN --> REM[Remap -1,1 to 0,1]
    
    REM --"Edge"--> STEP["Step<br/>(replaces Dissolve Amount)"]
    REM --"Edge1"--> SMOOTH["Smoothstep<br/>(replaces Dissolve Amount)"]
```

**Stretch Challenge B — Different Noise Patterns:** Replace Simple Noise with Gradient Noise or Voronoi for different dissolution patterns.

```mermaid
flowchart LR
    subgraph "Option 1"
        GN["Gradient Noise<br/>(directional pattern)"]
    end
    
    subgraph "Option 2"
        VOR["Voronoi"] --"Cells output"--> VN["Cellular pattern"]
    end
    
    GN --> STEP["Step<br/>(replaces Simple Noise)"]
    VN --> STEP
```

---

## Shader 5: Rim Highlight (10 min) — Stretch Goal

**Use cases:** Selection highlight, magical auras, stylized lighting

```mermaid
flowchart LR
    subgraph Properties
        MT["Texture2D<br/>Main Texture"]
        RC["Color HDR<br/>Rim Color"]
        RI["Float<br/>Rim Intensity"]
    end

    MT --> ST[Sample Texture 2D]
    
    FRES["Fresnel Effect<br/>(Power 3-5)"]
    
    FRES --> MUL1[Multiply]
    RC --> MUL1
    MUL1 --> MUL2[Multiply]
    RI --> MUL2
    
    ST --> ADD[Add]
    MUL2 --> ADD
    
    ADD --> BC["Base Color<br/>(Master Stack)"]
```

**Build Steps:**
1. Create Unlit Shader Graph
2. Add properties: Main Texture (Texture2D), Rim Color (Color, HDR), Rim Intensity (Float)
3. Sample Main Texture
4. Fresnel Effect (Power 3-5)
5. Multiply: Fresnel × Rim Color × Rim Intensity
6. Add to sampled texture → Base Color output

**Stretch Challenge — Pulsing Rim:**

```mermaid
flowchart LR
    subgraph Properties
        RI["Float<br/>Rim Intensity"]
    end

    TIME[Time] --> SIN[Sine]
    SIN --> REM["Remap -1,1 to 0,1<br/>(or Absolute)"]
    
    REM --> MUL[Multiply]
    RI --> MUL
    
    MUL --> REST["Multiply with<br/>Fresnel × Rim Color"]
```
