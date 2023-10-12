# Game Programmer Test (Abylight Studios, 2023)
- [Time of Completion](#time-of-completion)
- [Task 6: Theoric evaluation](#task-6-theoric-evaluation)

## Time of Completion

- Tasks 1 & 2: 1h30m
- Task 3: 3h
- Tasks 4 & 5: 3h 30m
- Task 6: 20m
- Documentation: 20m
- Total: 8h40m

## Task 6: Theoric evaluation

### 1. How could we save the use of a texture on the shader of Task 4 and still maintain its functionality?

We could assign a fixed value rather than requiring a texture. It could be used for inverting all the colors or a range.
In TestShader we'd have to change this:
```half4 frag(v2f i) : SV_Target
{
    half4 col = tex2D(_MainTex, i.uv);
    half maskValue = tex2D(_MaskTex, i.uv).r;

    half4 invertedCol = half4(1, 1, 1, 1) - col; // invert the color
    half4 finalColor = lerp(invertedCol, col, maskValue); // linearly interpolate between inverted and original color based on the mask value

    return finalColor;
}
```

to this:

```half4 frag(v2f i) : SV_Target
    {
        half4 col = tex2D(_MainTex, i.uv);

        // Instead of using the mask value to determine where to invert, 
        // we now directly invert every color.
        half4 invertedCol = half4(1, 1, 1, 1) - col; 

        return invertedCol; // Return the inverted color directly without any mask-based logic
    }
```

### 2. If we have 4 objects with different textures, but they all use the same shader, how could we render all 4 with just one Draw Call?

We could take advantage of Unity's [batching](https://docs.unity3d.com/Manual/DrawCallBatching.html). Unity employs an advanced mesh combination strategy for static objects to minimize draw calls. Also, the platform's dynamic batching capability can be used for dynamic objects with a restricted vertex count to achieve similar efficiency, but static batching is more efficient.

### 3. We have to optimize a scene for a console that has 2GB RAM and the profiler shows the following usage data:
### ○ a. 1 million triangles per frame
### ○ b. 1000 draw calls per frame
### ○ c. 1.9 GB of used memory
### If you could only dedicate time to optimize one of these factors, which one would be and why?

I'd prioritize optimizing the memory usage. Using up all the RAM can lead to crashes or glitches because there's no memory for other tasks. With only 2GB of RAM, which isn't a lot these days, my first move would be to hunt down any memory leaks using the Profiler. Depending on the game, I'd also look into things like dynamic loading or using Addressables to save on memory and potentially some other mentioned issues along the way.
