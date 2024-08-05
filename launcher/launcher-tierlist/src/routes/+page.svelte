<script lang="ts">
	import { onMount } from "svelte";
	import LauncherIcon from "../components/LauncherIcon.svelte";

    onMount(() => {
        var root: HTMLElement | null = document.querySelector(':root');

        if (root == null) return


        if (window.innerWidth < 1000) {
            root.style.setProperty('--dimension', '160px')
        } else {
            root.style.setProperty('--dimension', '200px')
        }

        window.onresize = () => {
            if (window.innerWidth < 1000) {
                root.style.setProperty('--dimension', '160px')
            } else {
                root.style.setProperty('--dimension', '200px')
            }
        }
    })

</script>

<div id="icons">
    <div id="group-1" class="row-group group">
        <div id="title-container" class="title">
            <h1 id="title-1" class="title">S</h1>
        </div>
        <div id="icons-1" class="group icon-group">
            <LauncherIcon iconName="modrinth" title="Modrinth" link="https://modrinth.app/" color="#1CD96A" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="prism" title="Prism" link="https://prismlauncher.org/" color="#99CD61" description="Expandable, performant, simple, open source. This launcher is bare bones, while still allowing access to all mods ever created."/>
        </div>
    </div>
    <div id="group-2" class="row-group group">
        <div id="title-container" class="title">
            <h1 id="title-2" class="title">A</h1>
        </div>        
        <div id="icons-2" class="group icon-group">
            <LauncherIcon iconName="gdlauncher" title="GDLauncher" link="https://minecraft.net/" color="#375076" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="multimc" title="MultiMC" link="https://multimc.org/" color="#7b5437" description="Expandable, performant, simple, open source. This launcher is bare bones, while still allowing access to all mods ever created."/>
            <LauncherIcon iconName="atlauncher" title="ATLauncher" link="https://multimc.org/" color="#89c236" description="Expandable, performant, simple, open source. This launcher is bare bones, while still allowing access to all mods ever created."/>
            <LauncherIcon iconName="minecraft launcher" title="Official" link="https://minecraft.net/" color="#52A535" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
        </div>
    </div>
    <div id="group-3" class="row-group group">
        <div id="title-container" class="title">
            <h1 id="title-3" class="title">B</h1>
        </div>
        <div id="icons-3" class="group icon-group">
            <LauncherIcon iconName="technic" title="Technic" link="https://prismlauncher.org/" color="#1389d2" description="Expandable, performant, simple, open source. This launcher is bare bones, while still allowing access to all mods ever created."/>
            <LauncherIcon iconName="curseforge" title="CurseForge" link="https://curseforge.com/" color="#f16436" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
        </div>
    </div>
    <div id="group-4" class="row-group group">
        <div id="title-container" class="title">
            <h1 id="title-4" class="title">C</h1>
        </div>
        <div id="icons-4" class="group icon-group">
            <LauncherIcon iconName="featherclient" title="Feather" link="https://minecraft.net/" color="#132938" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="badlionclient" title="Badlion" link="https://badlion.net/" color="#2caddc" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="lunarclient" title="Lunar" link="https://badlion.net/" color="#616161" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
        </div>
    </div>
    <div id="group-5" class="row-group group">
        <div id="title-container" class="title">
            <h1 id="title-5" class="title">F</h1>
        </div>
        <div id="icons-5" class="group icon-group">
            <LauncherIcon iconName="tlauncher" title="TLauncher" link="https://minecraft.net/" color="#38a5d5" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="sklauncher" title="SKLauncher" link="https://minecraft.net/" color="#3b4999" description="With developers being well paid, an easy to use interface, and being completely ad free, Modrinth is one of the best launchers you could use."/>
            <LauncherIcon iconName="easymc" title="EasyMC" link="https://prismlauncher.org/" color="#14826c" description="Expandable, performant, simple, open source. This launcher is bare bones, while still allowing access to all mods ever created."/>
        </div>
    </div>
</div>

<style lang="scss">
    :global(body) {
        background-color: white;
    }

    #icons {
        margin: 20px;
        margin-top: 10px;
        margin-bottom: 10px;
    }

    .group {
        display: flex;
        align-content: center;
        align-items: center;
        gap: 50px;
        padding: 20px;
        border-radius: 70px;
        width: 100%;
        // overflow: scroll;
        // flex: 0 0 auto;

        @media (max-width: 600px) {
            flex-wrap: wrap;
            justify-content: center;
            border-radius: 50px;

            width: unset;
        }
    }

    .row-group {
        padding: 10px;
        padding-right: 20px;

        @media (max-width: 600px) {
            flex-direction: column;

            gap: 15px;
            padding-bottom: 30px;
        }
    }

    #icons-1 {
        border: 10px solid lightcoral;
        background-color: lightcoral;

    }

    #icons-2 {
        border: 10px solid sandybrown;
        background-color: sandybrown;
    }

    #icons-3 {
        border: 10px solid lightgreen;
        background-color: lightgreen;
    }

    #icons-4 {
        border: 10px solid lightskyblue;
        background-color: lightskyblue;
    }

    #icons-5 {
        border: 10px solid orchid;
        background-color: orchid;
    }

    h1 {
        font-family: Gabarito;
        font-weight: 1000;
        font-size: 100px;
        margin: 0;

        @media (max-width: 1000px) {
            font-size: 70px;
            text-align: center;
        }
    }

    .title {
        display: inline-block;
        width: 68.5px;
    }

    #title-1 {
        color: lightcoral;
    }

    #title-2 {
        color: sandybrown;
    }

    #title-3 {
        color: lightgreen; 
    }

    #title-4 {
        color: lightskyblue
    }

    #title-5 {
        color: orchid;
    }
</style>