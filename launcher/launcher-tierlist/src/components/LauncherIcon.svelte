<script lang="ts">
    export let iconName: string;
    export let title: string;
    export let description: string;
    export let link: string;
    export let color: string;

	import FlippableCard from "../components/FlippableCard.svelte";
	import Open from "./glyphs/Open.svelte";
</script>

<FlippableCard color={color}>
    <div slot="logo" id="logo-container">
        {#await import(`$lib/assets/${iconName}.png`) then { default: src }}
            <img {src} alt={iconName}/>
        {/await}
    </div>
    <div slot="back" id="back-container">
        <div id="information-container">
            <div id="top">
                <h1 id="title">{title}</h1>
                <p id="description">{description}</p>
            </div>
            <div id="link">
                <div id="open-container">
                    <Open/>
                </div>
                <div id="link-container">
                    <a target="_blank" href={link}>{link}</a>
                </div>
            </div>
        </div>
    </div>
</FlippableCard>

<style lang="scss">
    #back-container {
        height: 100%;
    }

    #logo-container {
        display: flex;
        justify-content: center;
        align-items: center;

        height: 100%;
    }

    img {
        height: calc(var(--dimension) - 60px);
        width: calc(var(--dimension) - 60px);

        border-radius: 20px;

        @media (max-width: 1000px) {
            border-radius: 15px;
        }
    }

    #information-container {
        padding: 20px;
        height: calc(100% - 40px);

        display: flex;
        flex-direction: column;
        justify-content: space-between;

        @media (max-width: 1000px) {
            padding: 15px;
            height: calc(100% - 30px);
        }
    }

    h1, p, a {
        margin: 0;
        font-family: 'Gabarito';
    }

    h1 {
        font-weight: 1000;
        font-size: 27px;

        @media (max-width: 1000px) {
            font-size: 20px;
        }
    }

    p, a {
        font-weight: 100;
        color: #8F8F8F;
        font-size: 12px;

        @media (max-width: 1000px) {
            font-size: 10px;
        }
    }

    a {
        all: inherit;
        font-size: 12px;
        display: inline-block;

        &:hover {
            cursor: pointer;
        }

        @media (max-width: 1000px) {
            font-size: 10px;
            line-height: 10px;
        }
    }

    #link {
        display: flex;
        align-items: center;
        justify-items: center;
        align-content: center;
        gap: 5px;

        transition: transform 0.2s ease;

        &:active {
            transform: scale(0.97);
        }
    }

    #open-container {
        height: 15px;

        @media (max-width: 1000px) {
            // height: 10px;
        }
    }

    #link-container {
        height: 15px;
        font-family: 'Gabarito';
        font-weight: 100;
        color: #8F8F8F;
        font-size: 12px;
        text-decoration: underline;

        @media (max-width: 1000px) {
            font-size: 10px;
            height: 10px;
        }
    }
</style>