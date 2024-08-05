<script lang="ts">
    export let color: string

    let flipped = false;
    let card: HTMLElement
</script>

<div id="card-container" role="button" on:mouseover={() => {flipped = true}} on:mouseout={() => {flipped = false}} tabindex="0" bind:this={card} on:focus={() => {}} on:blur={() => {}}>
    <div class="card" class:flipped={flipped}>
        <div class="card-front" style="outline: 10px solid {color};">
            <slot name="logo"/>
        </div>
        <div class="card-back" style="outline: 10px solid {color};" >
            <slot name="back"/>
        </div>
    </div>
</div>


<style>
    #card-container {
        height: var(--dimension);
        width: var(--dimension);
    }

    .card {
        height: var(--dimension);
        width: var(--dimension);
        
        perspective: 2000px;
        position: relative;
        
        overflow: visible
    }

    .card-front, .card-back {
        width: 100%;
        height: 100%;
        position: absolute;
        backface-visibility: hidden;
        transition: transform 1s;
        transform-style: preserve-3d;
        overflow: visible;

        background-color: white;
        border-radius: 40px;
        box-shadow: 11px 11px 20px rgba(0, 0, 0, 0.1), 11px 11px 20px rgba(0, 0, 0, 0.08);

        @media (max-width: 1000px) {
            border-radius: 30px;

        }
    }

    .card-back {
        transform: rotateY(180deg);
    }

    .card.flipped .card-front {
        transform: rotateY(180deg);
    }

    .card.flipped .card-back {
        transform: rotateY(0);
    }
</style>
